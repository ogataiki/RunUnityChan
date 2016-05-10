using UnityEngine;
using System.Collections;

public class UnityChanController : MonoBehaviour {

    private Animator animator;
    private bool isJump = false;
    private float JumpTimeBase = 0.8f; // 秒
    private float JumpTime = 0.8f; // 秒
    private float JumpingTime = 0.0f;
    private float JumpHeightMax = 0.85f;
    private float JumpHeightMin = 0.2f;
    private float JumpHeight = 0.0f;
    private float BaseY = 0.0f;

    private float touchTime = 0.0f;

    [SerializeField]
    public AudioClip audioClipJump;
    [SerializeField]
    public AudioClip audioClipLanding;
    [SerializeField]
    public AudioClip audioClipCatCry1;
    [SerializeField]
    public AudioClip audioClipCatCry2;

    private AudioSource audioSourceSE;
    private AudioSource audioSourceCatCry;


    // Use this for initialization
    void Start () {

        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        audioSourceSE       = audioSources[0];
        audioSourceCatCry   = audioSources[1];

        animator = GetComponent<Animator>();

        BaseY = gameObject.transform.position.y;

        InitializeParametor();
    }

    void InitializeParametor()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, BaseY, gameObject.transform.position.z);

        animator.SetBool("OnGround", true);
        animator.SetBool("OnTap", false);
        animator.SetBool("GameOver", false);
        animator.SetBool("PreJump", false);
    }

    // Update is called once per frame
    void Update () {

        if(animator.GetBool("GameOver"))
        {
            return;
        }

        JumpTransform();  
    }

    public void GameStartTrigger()
    {
        animator.SetTrigger("GameStart");
    }

    public void IsTitle(bool value)
    {
        InitializeParametor();

        animator.SetBool("IsTitle", value);
    }

    public void ResultTrigger(string s)
    {
        InitializeParametor();

        animator.SetTrigger(s);
    }

    public void OnTapBegan()
    {
        if (animator.GetBool("OnGround") && animator.GetBool("OnTap") == false)
        {
            animator.SetBool("OnTap", true);
            animator.SetBool("PreJump", false);
            touchTime = Time.time;
            audioSourceSE.clip = audioClipJump;
        }
    }

    public void OnTapping()
    {
        if (animator.GetBool("OnGround") && animator.GetBool("OnTap"))
        {
            animator.SetBool("PreJump", true);
        }
        else if (animator.GetBool("OnGround") && animator.GetBool("OnTap") == false)
        {
            // タップしたことにする
            animator.SetBool("OnTap", true);
            animator.SetBool("PreJump", false);
            touchTime = Time.time;
            audioSourceSE.clip = audioClipJump;
        }
    }

    public void OnTapped() {
        if(animator.GetBool("OnTap"))
        {
            audioSourceSE.Play();

            float tappedTime = (Time.time - touchTime);
            JumpHeight = JumpHeightMin + tappedTime;
            if (JumpHeight >= JumpHeightMax)
            {
                JumpHeight = JumpHeightMax;
            }
            JumpTime = JumpTimeBase + (JumpHeight * 0.4f);
            animator.SetBool("OnTap", false);
            animator.SetBool("PreJump", false);
            isJump = true;
            touchTime = 0.0f;
            JumpTransformStart();
        }
    }

    private void JumpTransformStart()
    {
        animator.SetBool("OnGround", false);
        JumpingTime = 0.0f;
    }

    private void JumpTransform()
    {
        if(!animator.GetBool("OnGround"))
        {
            JumpingTime += Time.deltaTime;
            float diffY = (BaseY + JumpHeight) - gameObject.transform.position.y;
            float moveY;
            if (JumpingTime < JumpTime*0.5)
            {
                // 上昇中
                float p = diffY * 0.2f;
                moveY = gameObject.transform.position.y + p;
            }
            else
            {
                audioSourceSE.clip = audioClipLanding;

                // 下降中
                float m = diffY * 0.3f;
                moveY = gameObject.transform.position.y - m;
            }
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, moveY, gameObject.transform.position.z);

            if (JumpingTime > JumpTime || (JumpingTime > JumpTime*0.5 && gameObject.transform.position.y <= BaseY + JumpHeightMin * 0.6f))
            {
                isJump = false;
                animator.SetBool("OnGround", true);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, BaseY, gameObject.transform.position.z);

                audioSourceSE.Play();
            }
        }
    }

    public void OnCollidedWithObstacle()
    {
        animator.SetTrigger("Collision");
        animator.SetBool("GameOver", true);

        audioSourceCatCry.clip = audioClipCatCry1;
        audioSourceCatCry.Play();
    }

    public void OnCollidedWithCake()
    {
        audioSourceCatCry.clip = audioClipCatCry2;
        audioSourceCatCry.Play();
    }

    public void OnCollidedWithRareCake()
    {
        audioSourceCatCry.clip = audioClipCatCry2;
        audioSourceCatCry.Play();
    }

    public void OnCallChangeFace() {
	}
}
