using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using Uween;

public class MainSceneController : MonoBehaviour {

    [SerializeField]
    private GameObject mainCamera;
    private MainCameraController mainCameraController;
    [SerializeField]
    private GameObject subCamera;

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
    private UnityChanController unityChanController;

    [SerializeField]
    private float gameSpeed = 0.8f;

    [SerializeField]
    private float gameSpeedAdd = 0.02f;

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
    private float bonusSpeed_ratio = 0.02f;

    [SerializeField]
    private GameObject particleBonusGetPrefab;

    private float touchTime = 0.0f;

    private int createObstacleCount = 0;

    private int getBonusTotal = 0;
    private int getBonusCount = 0;
    private int getBonusSeries = 0;

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
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start () {

        textTopScore.text = "Top Score : " + PlayerPrefs.GetInt(scoreTopSavePath, 0);

        InitializeParametor();

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
    }

    void InitializeParametor()
    {
        createObstacleCount = 0;
        getBonusCount = 0;
        getBonusTotal = 0;
        getBonusSeries = 0;

        nextScaleX = nextScaleXMax;
        nextScaleY = nextScaleYMin;
        nextScaleZ = nextScaleZMin;

        nextTime_bonus = Random.Range(nextTimeMin_bonus, nextTimeMax_bonus);
        nextSpeed_bonus = bonusSpeed_base;
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
    }

    void UpdateRunGame()
    {
        TouchInfo info = TouchUtil.GetTouch();
        if (info == TouchInfo.Began)
        {
            // タッチ開始
            //Debug.Log("TapBegan");
            unityChanController.OnTapBegan();
            touchTime = Time.time;
        }
        else if (info == TouchInfo.Ended || info == TouchInfo.Canceled)
        {
            // タッチ終了
            //Debug.Log("TapEnded");
            unityChanController.OnTapped();
            touchTime = 0.0f;
        }
        else if (touchTime > 0.0f || info == TouchInfo.Stationary || info == TouchInfo.Moved)
        {
            //Debug.Log("OnTapping");
            unityChanController.OnTapping();
        }

        elapsedTime += Time.deltaTime;
        if (nextTime <= elapsedTime)
        {
            GameObject obstacle = Instantiate(this.obstaclePrefab);
            obstacle.transform.localScale = new Vector3(nextScaleX, nextScaleY, nextScaleZ);
            obstacle.transform.position = new Vector3(0.0f, obstacle.GetComponent<Renderer>().bounds.size.y * 0.5f, 3.0f);
            ObstacleController obstacleController = obstacle.GetComponent<ObstacleController>();
            obstacleController.SetSpeed(gameSpeed + ((float)getBonusSeries) * gameSpeedAdd);
            Debug.Log("obstacleSpeed:" + gameSpeed + ((float)getBonusSeries) * gameSpeedAdd);
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

        elapsedTime_bonus += Time.deltaTime;
        if (nextTime_bonus <= elapsedTime_bonus)
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
            elapsedTime_bonus = 0.0f;
            nextTime_bonus = Random.Range(nextTimeMin_bonus, nextTimeMax_bonus);
            nextHeight_bonus = Random.Range(nextHeightMin_bonus, nextHeightMax_bonus);
            nextSpeed_bonus = Random.Range(bonusSpeed_base-(getBonusSeries* bonusSpeed_ratio), bonusSpeed_base + ((getBonusSeries * bonusSpeed_ratio)*2));
            Debug.Log("nextSpeed_bonus:"+ nextSpeed_bonus);
        }

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
        Speed(gameSpeed + ((float)getBonusSeries) * gameSpeedAdd);
        Debug.Log("GameSpeed:"+ gameSpeed + ((float)getBonusSeries) * gameSpeedAdd);
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

            if ((getBonusTotal > PlayerPrefs.GetInt(scoreTopSavePath, 0)))
            {
                textResultGoodComment.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                ResultGoodCommentFadeOut();
                unityChanController.ResultTrigger("ToResultGood");
            }
            else
            {
                textResultGoodComment.color = new Color(1.0f, 1.0f, 0.0f, 0.0f);
                unityChanController.ResultTrigger("ToResultBad");
            }
        }
    }

    void ResultGoodCommentFadeOut()
    {
        if(sts == SceneStatus.Result)
        {
            TweenA.Add(textResultGoodComment.gameObject, 0.2f, 0.0f).Then(ResultGoodCommentFadeIn);
        }
    }

    void ResultGoodCommentFadeIn()
    {
        if (sts == SceneStatus.Result)
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
                if (getBonusTotal > PlayerPrefs.GetInt(scoreTopSavePath, 0))
                {
                    PlayerPrefs.SetInt(scoreTopSavePath, getBonusTotal);
                }

                // debug
                // PlayerPrefs.SetInt(scoreTopSavePath, 0);
                // debug

                textTopScore.text = "Top Score : " + PlayerPrefs.GetInt(scoreTopSavePath, 0);


                unityChanController.IsTitle(true);

                uiResultScenePanel.SetActive(false);
                uiGameScenePanel.SetActive(false);
                uiTitleScenePanel.SetActive(true);

                mainCameraController.ChangePattern(0);

                touchTime = 0.0f;

                sts = SceneStatus.Title;
            }

            isPreChangeTitle = false;
        }
    }

    // Update is called once per frame
    void Update () {
        frameCount++;

        switch(sts)
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
        unityChanController.OnCollidedWithObstacle();

        Stop(true);

        sts = SceneStatus.GameOver;
    }

    private void BonusCollidedWithUnityChan(Vector3 position)
    {
        if (sts != SceneStatus.RunGame)
        {
            return;
        }

        GameObject p = Instantiate(this.particleBonusGetPrefab);
        p.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

        // 得点加算
        Debug.Log("!!bonus!!");
        getBonusCount++;
        getBonusSeries++;
        getBonusTotal += getBonusCount * getBonusSeries;

        textNowScore.text = scoreNowTitle + getBonusTotal;
        textGetCakes.text = cakesTitle + getBonusCount;
        textNowCombo.text = comboTitle + getBonusSeries;

        UpdateSpeed();
    }

    private void ThroughBonus()
    {
        if (sts != SceneStatus.RunGame)
        {
            return;
        }

        Debug.Log("!!Through!!");
        getBonusSeries = 0;
        Debug.Log("series:" + getBonusSeries);
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
}
