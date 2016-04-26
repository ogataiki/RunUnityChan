using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {

    private GameObject runCat;

    struct CAMERA_PATTERN
    {
        public float p_x, p_y, p_z;
        public float a_x, a_y, a_z;
        public bool toFollow;

        public CAMERA_PATTERN(float p_x, float p_y, float p_z, float a_x, float a_y, float a_z, bool toFollow)
        {
            this.p_x = p_x;
            this.p_y = p_y;
            this.p_z = p_z;
            this.a_x = a_x;
            this.a_y = a_y;
            this.a_z = a_z;
            this.toFollow = toFollow;
        }
    }

    [SerializeField]
    private CAMERA_PATTERN[] pattern = new CAMERA_PATTERN[] {
        new CAMERA_PATTERN(0.6f, 0.4f, 0.9f, 0.0f, 220.0f, 0.0f, true),
        new CAMERA_PATTERN(1.2f, 0.8f, 2.1f, 0.0f, 220.0f, 0.0f, false),
        new CAMERA_PATTERN(1.8f, 1.2f, 3.4f, 0.0f, 220.0f, 0.0f, false),
        new CAMERA_PATTERN(0.6f, 0.5f, 0.9f, 0.0f, 220.0f, 0.0f, true),
    };

    [SerializeField]
    private int nowCameraPattern = 0;

    private Vector3 basePosition;

    // Use this for initialization
    void Start () {
        runCat = GameObject.Find("RunCat");
        basePosition = this.gameObject.transform.position;
    }

    public void OnClick_ChangePattern()
    {
        ChangePattern(-1);
    }

    public void ChangePattern(int pattern)
    {
        if (pattern < 0)
        {
            nowCameraPattern = (nowCameraPattern >= 2) ? 0 : nowCameraPattern + 1;
        }
        else if (pattern <= 3)
        {
            nowCameraPattern = pattern;
        }
        Debug.Log("pattern:" + nowCameraPattern);
    }

    // Update is called once per frame
    void Update () {
        CAMERA_PATTERN p = pattern[nowCameraPattern];
        if (p.toFollow)
        {
            this.gameObject.transform.position = Vector3.Lerp(
                this.gameObject.transform.position, 
                new Vector3(runCat.transform.position.x + p.p_x, runCat.transform.position.y + p.p_y, runCat.transform.position.z + p.p_z), 
                20 * Time.deltaTime
                );
        }
        else
        {
            this.gameObject.transform.position = Vector3.Lerp(
                this.gameObject.transform.position,
                new Vector3(p.p_x, p.p_y, p.p_z),
                20 * Time.deltaTime
                );
        }
        this.gameObject.transform.localRotation = Quaternion.Euler(p.a_x, p.a_y, p.a_z);
    }
}
