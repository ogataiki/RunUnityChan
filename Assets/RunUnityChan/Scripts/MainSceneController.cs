using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using Uween;

public class MainSceneController : MonoBehaviour {

    private float dt = 0.02f;
    private bool isPause = false;

    [SerializeField]
    private GameObject mainCamera;
    private MainCameraController mainCameraController;
    [SerializeField]
    private GameObject subCamera;

    [SerializeField]
    private GameObject unityAds;
    private UnityAdsScript unityAdsScript;

    [SerializeField]
    public AudioClip audioClipTitle;
    [SerializeField]
    public AudioClip audioClipGame;
    [SerializeField]
    public AudioClip audioClipResultGood;
    [SerializeField]
    public AudioClip audioClipResultBad;

    [SerializeField]
    public AudioClip audioClipGetCake;

    private AudioSource audioSourceBGM;
    private AudioSource audioSourceSE;

    [SerializeField]
    private GameObject uiAllScenePanel;
    [SerializeField]
    private GameObject uiTitleScenePanel;
    [SerializeField]
    private GameObject uiGameScenePanel;
    [SerializeField]
    private GameObject uiResultScenePanel;

    [SerializeField]
    private Text textTopScore;

    [SerializeField]
    private Text textTutorial;

    private string[] tutorialString = { "Jump by Tap on Display", "HigherJump by LongTap"};
    private int tutorialStringIndex = 0;

    [SerializeField]
    private Text textNowScore;
    private string scoreNowTitle = "Score : ";

    [SerializeField]
    private Text textGetCakes;
    private string cakesTitle = "Cakes : ";

    [SerializeField]
    private Text textNowCombo;
    private string comboTitle = "Combo : ";

    [SerializeField]
    private Text textResultGoodComment;

    [SerializeField]
    private InputField textUserName;

    [SerializeField]
    private UnityChanController unityChanController;

    [SerializeField]
    private float gameSpeed = 0.8f;

    [SerializeField]
    private float gameSpeedAdd = 0.02f;

    private int gameSpeedCount = 0;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject obstaclePrefab;
    private float elapsedTime = 0.0f;
    private float nextTime = 0.0f;
    private float nextTimeMin = 1.5f;
    private float nextTimeMax = 3.0f;
    private float nextScaleX = 0.5f;
    private float nextScaleXMin = 0.1f;
    private float nextScaleXMax = 0.7f;
    private float nextScaleY = 0.5f;
    private float nextScaleYMin = 0.5f;
    private float nextScaleYMax = 2.5f;
    private float nextScaleZ = 0.5f;
    private float nextScaleZMin = 0.2f;
    private float nextScaleZMax = 0.8f;

    [SerializeField]
    private GameObject bonusPrefab;
    private float elapsedTime_bonus = 0.0f;
    private float nextTime_bonus = 0.0f;
    private float nextTimeMin_bonus = 1.0f;
    private float nextTimeMax_bonus = 5.0f;
    private float nextHeight_bonus = 0.05f;
    private float nextHeightMin_bonus = 0.05f;
    private float nextHeightMax_bonus = 0.6f;

    private float nextSpeed_bonus = 0.8f;
    [SerializeField]
    private float bonusSpeed_base = 0.8f;
    [SerializeField]
    private float bonusSpeed_ratio = 0.05f;

    [SerializeField]
    private GameObject particleBonusGetPrefab;


    [SerializeField]
    private GameObject rareBonusPrefab;
    [SerializeField]
    private float rareBonusProb = 5.0f;
    [SerializeField]
    private ParticleRareBonusGetController rareBonusGetController;
    [SerializeField]
    private float invincibleTime = 10.0f;
    private float invincibleTimeNow = 0.0f;
    private int invincibleCount = 0;
    private int firstInvincibleCount = 10;
    private bool isFirstInvincible = false;

    [SerializeField]
    private GameObject particleRareBonusGetPrefab;


    [SerializeField]
    private GameObject particleHanabiPrefab;
    private GameObject hanabi = null;

    private float touchTime = 0.0f;

    private int createObstacleCount = 0;

    private int getBonusTotal = 0;
    private int getBonusCount = 0;
    private int getBonusSeries = 0;
    private bool IsNowRecord = false;

    private string scoreTopSavePath         = "TopScore";

    private bool isPreChangeTitle = false;

    private enum SceneStatus { Title, RunGame, GameOver, Result };
    private SceneStatus sts = SceneStatus.Title;

    public event System.Action GoTitle = delegate { };
    public event System.Action<float> Speed = delegate { };
    public event System.Action<bool> Stop = delegate { };

    private int frameCount = 0;
    private float frameNextTime = 0.0f;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start() {

        dt = Time.deltaTime * 1f;

        if(Application.systemLanguage == SystemLanguage.Japanese)
        {
            DialogManager.Instance.SetLabel("いいよ", "だめ", "うるさい");
        }
        else
        {
            DialogManager.Instance.SetLabel("Yes", "No", "Close");
        }

        rareBonusGetController.ParticleStop();

        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        audioSourceBGM = audioSources[0];
        audioSourceSE = audioSources[1];

        Debug.Log("UserName:" + PlayerPrefs.GetString("UserName", "Guest"));
        textUserName.text = PlayerPrefs.GetString("UserName", "Guest");
        if (PlayerPrefs.GetString("UserName", "Guest") == "Guest")
        {
            PlayerPrefs.SetString("UserName", "Guest");
        }
        RankingManager.Instance.SighUp(PlayerPrefs.GetString("UserName", "Guest"));

        textTopScore.text = "Top Score : " + PlayerPrefs.GetInt(scoreTopSavePath, 0);

        InitializeParametor();

        unityAdsScript = unityAds.GetComponent<UnityAdsScript>();

        mainCameraController = mainCamera.GetComponent<MainCameraController>();
        mainCameraController.ChangePattern(0);

        subCamera.SetActive(false);

        uiGameScenePanel.SetActive(false);
        uiResultScenePanel.SetActive(false);

        unityChanController.IsTitle(true);

        for (int i = -9; i <= 3; i++)
        {
            CreateFloor((float)i);
        }
        Stop(true);

        frameNextTime = Time.time + 1.0f;

        audioSourceBGM.clip = audioClipTitle;
        audioSourceBGM.Play();
    }

    void InitializeParametor()
    {
        createObstacleCount = 0;
        getBonusCount = 0;
        getBonusTotal = 0;
        getBonusSeries = 0;

        gameSpeedCount = 0;
        invincibleCount = 0;

        nextScaleX = nextScaleXMax;
        nextScaleY = nextScaleYMin;
        nextScaleZ = nextScaleZMin;

        nextTime_bonus = Random.Range(nextTimeMin_bonus, nextTimeMax_bonus);
        nextSpeed_bonus = bonusSpeed_base;

        isFirstInvincible = false;
    }

    void UpdateTitle()
    {
        TouchInfo info = TouchUtil.GetTouch();
        if (info == TouchInfo.Began)
        {
            // GUIが被ってるばあいは処理しない(引数なしはマウス用　引数ありはタッチ用)
            if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }
            else
            {
                tutorialStringIndex = (tutorialStringIndex >= 1) ? 0 : tutorialStringIndex + 1;
                textTutorial.text = tutorialString[tutorialStringIndex];
            }

            // タッチ開始
            Debug.Log("TapBegan");
            unityChanController.OnTapBegan();
            touchTime = Time.time;
        }
        else if (info == TouchInfo.Ended || info == TouchInfo.Canceled)
        {
            if (touchTime > 0.0f)
            {
                // タッチ終了
                Debug.Log("TapEnded");
                unityChanController.OnTapped();
                touchTime = 0.0f;
            }
        }
        else if (touchTime > 0.0f || info == TouchInfo.Stationary || info == TouchInfo.Moved)
        {
            if (touchTime > 0.0f)
            {
                Debug.Log("OnTapping");
                unityChanController.OnTapping();
            }
        }
    }

    public void OnClick_GameStart()
    {
        if (touchTime > 0.0f)
        {
            // テストジャンプ中は処理しない
            return;
        }
        IsNowRecord = false;

        mainCameraController.ChangePattern(2);
        subCamera.SetActive(true);

        uiTitleScenePanel.SetActive(false);

        textNowScore.text = scoreNowTitle + 0;
        textGetCakes.text = cakesTitle + 0;
        textNowCombo.text = comboTitle + 0;
        uiGameScenePanel.SetActive(true);

        // タッチ開始
        InitializeParametor();
        unityChanController.IsTitle(false);
        unityChanController.GameStartTrigger();
        touchTime = 0.0f;
        Stop(false);
        Speed(gameSpeed);
        sts = SceneStatus.RunGame;

        audioSourceBGM.Stop();
        audioSourceBGM.clip = audioClipGame;
        audioSourceBGM.Play();
    }

    void UpdateRunGame()
    {
        TouchInfo info = TouchUtil.GetTouch();
        if (info == TouchInfo.Began)
        {
            // タッチ開始

            // GUIが被ってるばあいは処理しない(引数なしはマウス用　引数ありはタッチ用)
            if (EventSystem.current.IsPointerOverGameObject() == false && EventSystem.current.IsPointerOverGameObject(0) == false)
            {
                //Debug.Log("TapBegan");
                unityChanController.OnTapBegan();
                touchTime = Time.time;
            }
        }
        else if (info == TouchInfo.Ended || info == TouchInfo.Canceled)
        {
            // タッチ終了

            if (touchTime > 0.0f)
            {
                //Debug.Log("TapEnded");
                unityChanController.OnTapped();
                touchTime = 0.0f;
            }
        }
        else if (touchTime > 0.0f || info == TouchInfo.Stationary || info == TouchInfo.Moved)
        {
            if (touchTime > 0.0f)
            {
                //Debug.Log("OnTapping");
                unityChanController.OnTapping();
            }
        }

        elapsedTime += dt;
        if (nextTime <= elapsedTime)
        {
            GameObject obstacle = Instantiate(this.obstaclePrefab);
            obstacle.transform.localScale = new Vector3(nextScaleX, nextScaleY, nextScaleZ);
            obstacle.transform.position = new Vector3(0.0f, obstacle.GetComponent<Renderer>().bounds.size.y * 0.5f, 3.0f);
            ObstacleController obstacleController = obstacle.GetComponent<ObstacleController>();
            obstacleController.SetSpeed(gameSpeed + ((float)gameSpeedCount) * gameSpeedAdd);
            //Debug.Log("obstacleSpeed:" + gameSpeed + ((float)gameSpeedCount) * gameSpeedAdd);
            obstacleController.CollidedWithUnityChan += ObstacleCollidedWithUnityChan;
            obstacleController.PreDestroy += ObstaclePreDestroy;
            GoTitle += obstacleController.GoTitle;
            Speed += obstacleController.SetSpeed;
            Stop += obstacleController.Stop;
            elapsedTime = 0.0f;
            nextTime = Random.Range(nextTimeMin, nextTimeMax);
            nextScaleX = Random.Range(nextScaleXMin, nextScaleXMax);
            nextScaleY = Random.Range(nextScaleYMin, (nextScaleYMin+((float)getBonusCount * 0.05f) > nextScaleYMax) ? nextScaleYMax : nextScaleYMin + ((float)getBonusCount * 0.05f));
            nextScaleZ = Random.Range(nextScaleZMin, (nextScaleZMin + ((float)getBonusCount * 0.05f) > nextScaleZMax) ? nextScaleZMax : nextScaleZMin + ((float)getBonusCount * 0.05f));
            createObstacleCount++;
        }

        elapsedTime_bonus += dt;
        if (nextTime_bonus <= elapsedTime_bonus)
        {
            // 3%の確率
            if (isFirstInvincible == false && getBonusCount >= firstInvincibleCount)
            {
                isFirstInvincible = true;
                CreateRareBonus();
                invincibleCount = 0;
            }
            else if (getBonusCount >= firstInvincibleCount && getBonusSeries > 5 && invincibleTimeNow < 1.0f && Random.Range(1.0f, 100.0f) <= rareBonusProb)
            {
                CreateRareBonus();
                invincibleCount = 0;
            }
            else if (invincibleCount >= 30)
            {
                CreateRareBonus();
                invincibleCount = 0;
            }
            else
            {
                CreateBonus();
            }
            elapsedTime_bonus = 0.0f;
            nextTime_bonus = Random.Range(nextTimeMin_bonus, nextTimeMax_bonus);
            if (invincibleTimeNow > 0.0f)
            {
                nextTime_bonus *= 0.15f;
            }
            nextHeight_bonus = Random.Range(nextHeightMin_bonus, nextHeightMax_bonus);
            nextSpeed_bonus = Random.Range(bonusSpeed_base - (getBonusSeries * bonusSpeed_ratio), bonusSpeed_base + ((getBonusSeries * bonusSpeed_ratio) * 2));
            //Debug.Log("nextSpeed_bonus:" + nextSpeed_bonus);
        }

        if (invincibleTimeNow > 0.0f)
        {
            invincibleTimeNow -= (1.0f * dt);
        }
        if (rareBonusGetController != null && invincibleTimeNow < 2.0f)
        {
            // 実際の無敵時間より早めに終わらせる
            rareBonusGetController.ParticleStop();
        }
    }

    void CreateBonus()
    {
        GameObject bonus = Instantiate(this.bonusPrefab);
        bonus.transform.position = new Vector3(bonus.transform.position.x, nextHeight_bonus, bonus.transform.position.z + 4.0f);
        BonusController bonusController = bonus.GetComponent<BonusController>();
        bonusController.SetSpeed(nextSpeed_bonus);
        bonusController.CollidedWithUnityChan += BonusCollidedWithUnityChan;
        bonusController.ThroughedWithUnityChan += ThroughBonus;
        bonusController.PreDestroy += BonusPreDestroy;
        GoTitle += bonusController.GoTitle;
        Stop += bonusController.Stop;
    }

    void CreateRareBonus()
    {
        GameObject bonus = Instantiate(this.rareBonusPrefab);
        bonus.transform.position = new Vector3(bonus.transform.position.x, nextHeight_bonus, bonus.transform.position.z + 4.0f);
        BonusController bonusController = bonus.GetComponent<BonusController>();
        bonusController.SetSpeed(bonusSpeed_base);
        bonusController.CollidedWithUnityChan += RareBonusCollidedWithUnityChan;
        bonusController.ThroughedWithUnityChan += ThroughBonus;
        bonusController.PreDestroy += BonusPreDestroy;
        GoTitle += bonusController.GoTitle;
        Stop += bonusController.Stop;
    }

    void CreateFloor(float offset)
    {
        GameObject floor = Instantiate(this.floorPrefab);
        floor.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y, 1.0f * offset);
        FloorController floorController = floor.GetComponent<FloorController>();
        floorController.SetSpeed(gameSpeed + ((float)getBonusSeries) * gameSpeedAdd);
        floorController.PreDestroy += FloorPreDestroy;
        GoTitle += floorController.GoTitle;
        Speed += floorController.SetSpeed;
        Stop += floorController.Stop;
    }

    void UpdateSpeed()
    {
        Speed(gameSpeed + ((float)gameSpeedCount) * gameSpeedAdd);
        //Debug.Log("GameSpeed:"+ gameSpeed + ((float)gameSpeedCount) * gameSpeedAdd);
    }

    void UpdateGameOver()
    {
        TouchInfo info = TouchUtil.GetTouch();
        if (info == TouchInfo.Began)
        {
            GoTitle();
            for (int i = -50; i <= 2; i++)
            {
                CreateFloor(i);
            }
            Stop(true);

            mainCameraController.ChangePattern(0);

            subCamera.SetActive(false);

            uiResultScenePanel.SetActive(true);

            sts = SceneStatus.Result;

            audioSourceBGM.Stop();

            if ((getBonusTotal > PlayerPrefs.GetInt(scoreTopSavePath, 0)))
            {
                PlayerPrefs.SetInt(scoreTopSavePath, getBonusTotal);

                IsNowRecord = true;
                textResultGoodComment.gameObject.SetActive(true);
                textResultGoodComment.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                ResultGoodCommentFadeOut();
                unityChanController.ResultTrigger("ToResultGood");
                hanabi = Instantiate(this.particleHanabiPrefab);
                audioSourceBGM.clip = audioClipResultGood;
            }
            else
            {
                IsNowRecord = false;
                textResultGoodComment.gameObject.SetActive(false);
                unityChanController.ResultTrigger("ToResultBad");
                hanabi = null;
                audioSourceBGM.clip = audioClipResultBad;
            }

            audioSourceBGM.Play();

            RankingManager.Instance.SendRanking(RankingManager.RankingType.NekoRun_TopScore, getBonusTotal);
            RankingManager.Instance.SendRanking(RankingManager.RankingType.NekoRun_Cakes, getBonusCount);
            RankingManager.Instance.SendRanking(RankingManager.RankingType.NekoRun_Combo, getBonusSeries);

        }
    }
    private TweenA tween = null;
    void ResultGoodCommentFadeOut()
    {
        //Debug.Log("ResultGoodCommentFadeOut");
        if (IsNowRecord)
        {
            TweenA.Add(textResultGoodComment.gameObject, 0.2f, 0.0f).Then(ResultGoodCommentFadeIn);
        }
    }

    void ResultGoodCommentFadeIn()
    {
        //Debug.Log("ResultGoodCommentFadeIn");
        if (IsNowRecord)
        {
            TweenA.Add(textResultGoodComment.gameObject, 0.2f, 1.0f).Then(ResultGoodCommentFadeOut);
        }
    }

    void UpdateResult()
    {
        TouchInfo info = TouchUtil.GetTouch();
        if (info == TouchInfo.Began)
        {
            isPreChangeTitle = true;
        }
        else if (info == TouchInfo.Ended || info == TouchInfo.Canceled)
        {
            if (isPreChangeTitle)
            {
                // タッチ開始

                if (IsNowRecord)
                {
                    string dialog_message = "";
                    if (Application.systemLanguage == SystemLanguage.Japanese)
                    {
                        dialog_message = "ハイスコアおめでとう！\n少しの間、他のゲームのPRをさせてください！";
                    }
                    else
                    {
                        dialog_message = "Congratulations!\nCould you see the Game advertising？";
                    }
                    DialogManager.Instance.ShowSelectDialog(
                        dialog_message,
                        (bool result) => {
                            if (result == true)
                            {
                                // YESの場合のみ、ここの処理が走る。
                                unityAdsScript.ShowAd();
                            }
                        }
                    );
                }

                // debug
                //PlayerPrefs.SetInt(scoreTopSavePath, 0);
                // debug

                textTopScore.text = "Top Score : " + PlayerPrefs.GetInt(scoreTopSavePath, 0);

                if(hanabi != null)
                {
                    Destroy(hanabi);
                    hanabi = null;
                }

                unityChanController.IsTitle(true);

                uiResultScenePanel.SetActive(false);
                uiGameScenePanel.SetActive(false);
                uiTitleScenePanel.SetActive(true);

                mainCameraController.ChangePattern(0);

                touchTime = 0.0f;

                sts = SceneStatus.Title;

                audioSourceBGM.Stop();
                audioSourceBGM.clip = audioClipTitle;
                audioSourceBGM.Play();
            }

            isPreChangeTitle = false;
        }
    }

    // Update is called once per frame
    void Update () {
        frameCount++;

        if (isPause)
        {
            return;
        }

        switch (sts)
        {
            case SceneStatus.Title:
                UpdateTitle();
                break;
            case SceneStatus.RunGame:
                UpdateRunGame();
                break;
            case SceneStatus.GameOver:
                UpdateGameOver();
                break;
            case SceneStatus.Result:
                UpdateResult();
                break;
            default:
                break;
        }

        if(Time.time >= frameNextTime)
        {
            //Debug.Log("FPS : " + frameCount);
            frameCount = 0;
            frameNextTime += 1.0f;
        }
    }

    private void ObstacleCollidedWithUnityChan()
    {
        if(sts != SceneStatus.RunGame)
        {
            return;
        }
        if(invincibleTimeNow > 0.0f)
        {
            return;
        }
        unityChanController.OnCollidedWithObstacle();

        Stop(true);

        audioSourceBGM.Stop();

        gameSpeedCount = 0;

        sts = SceneStatus.GameOver;
    }

    private void BonusCollidedWithUnityChan(Vector3 position)
    {
        Debug.Log("GetBonus!!!");
        if (sts != SceneStatus.RunGame)
        {
            return;
        }
        unityChanController.OnCollidedWithCake();

        GameObject p = Instantiate(this.particleBonusGetPrefab);
        p.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

        // 得点加算
        //Debug.Log("!!bonus!!");
        getBonusCount++;
        getBonusSeries++;
        getBonusTotal += getBonusCount * getBonusSeries;

        gameSpeedCount++;
        invincibleCount++;

        textNowScore.text = scoreNowTitle + getBonusTotal;
        textGetCakes.text = cakesTitle + getBonusCount;
        textNowCombo.text = comboTitle + getBonusSeries;

        audioSourceSE.clip = audioClipGetCake;
        audioSourceSE.Play();

        UpdateSpeed();
    }

    private void RareBonusCollidedWithUnityChan(Vector3 position)
    {
        Debug.Log("GetRareBonus!!!");
        if (sts != SceneStatus.RunGame)
        {
            return;
        }
        unityChanController.OnCollidedWithRareCake();

        invincibleCount = 0;

        rareBonusGetController.ParticleStart();

        audioSourceSE.clip = audioClipGetCake;
        audioSourceSE.Play();

        gameSpeedCount = 0;

        invincibleTimeNow = invincibleTime;

        nextTime_bonus = 0.0f;
    }

    private void ThroughBonus()
    {
        if (sts != SceneStatus.RunGame)
        {
            return;
        }

        Debug.Log("!!BonusThrough!!");
        getBonusSeries = 0;
        //Debug.Log("series:" + getBonusSeries);
    }

    private void ObstaclePreDestroy(GameObject obstacle)
    {
        ObstacleController obstacleController = obstacle.GetComponent<ObstacleController>();
        GoTitle -= obstacleController.GoTitle;
        Speed -= obstacleController.SetSpeed;
        Stop -= obstacleController.Stop;
        Destroy(obstacle);
    }

    private void FloorPreDestroy(GameObject floor)
    {
        FloorController floorController = floor.GetComponent<FloorController>();
        GoTitle -= floorController.GoTitle;
        Speed -= floorController.SetSpeed;
        Stop -= floorController.Stop;
        Destroy(floor);
    }


    private void BonusPreDestroy(GameObject bonus)
    {
        BonusController bonusController = bonus.GetComponent<BonusController>();
        GoTitle -= bonusController.GoTitle;
        Stop -= bonusController.Stop;
        Destroy(bonus);
    }

    
    public void OnInputUserName()
    {
        string s = textUserName.text;
        Debug.Log("UserName:"+s);
        if(s != "")
        {
            PlayerPrefs.SetString("UserName", s);

            RankingManager.Instance.ChangeName(s);
        }
    }

    public void ShowRanking()
    {
        RankingManager.Instance.ShowRanking();
    }

    public void OnPause()
    {
        if(isPause)
        {
            isPause = false;
            Time.timeScale = 1;
        }
        else
        {
            isPause = true;
            Time.timeScale = 0;
        }
    }
}
