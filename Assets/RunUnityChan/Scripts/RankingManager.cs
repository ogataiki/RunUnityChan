using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Kayac.Lobi.SDK;

/// <summary>
/// Lobiを使ったランキング周りを管理するマネージャクラス(シングルトン)
/// </summary>
public class RankingManager : SingletonMonoBehaviour<RankingManager>
{

    //ランキングの種類(兼ID)
    public enum RankingType
    {
        NekoRun_TopScore = 0, NekoRun_Cakes, NekoRun_Combo
    }

    //ランキングに登録する初期値
    private static readonly Dictionary<RankingType, int> RANKING_INITIAL_VALUE = new Dictionary<RankingType, int>() {
    {RankingType.NekoRun_TopScore, 0}, {RankingType.NekoRun_Cakes, 0}, {RankingType.NekoRun_Combo, 0},
  };

    //ランキングからデータを取得していない時の数値
    private const int NOT_GET_RANKING_DATA_VALUE = -1;

    //ランキング参加人数
    private Dictionary<RankingType, int> _joinCountList;
    public Dictionary<RankingType, int> JoinCountList
    {
        get { return _joinCountList; }
    }

    //自分の順位
    private Dictionary<RankingType, int> _myRankList;
    public Dictionary<RankingType, int> MyRankList
    {
        get { return _myRankList; }
    }

    //ランキングに登録されている自分のハイスコア
    private Dictionary<RankingType, int> _highScoreList;
    public Dictionary<RankingType, int> HighScoreList
    {
        get { return _highScoreList; }
    }

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        //ランキングに登録されているデータのリスト
        _joinCountList = new Dictionary<RankingType, int>();
        _myRankList = new Dictionary<RankingType, int>();
        _highScoreList = new Dictionary<RankingType, int>();

        foreach (RankingType rankingType in Enum.GetValues(typeof(RankingType)))
        {
            _joinCountList[rankingType] = NOT_GET_RANKING_DATA_VALUE;
            _myRankList[rankingType] = NOT_GET_RANKING_DATA_VALUE;
            _highScoreList[rankingType] = NOT_GET_RANKING_DATA_VALUE;
        }

    }

    private void Start()
    {
        SighUp(PlayerPrefs.GetString("UserName", "Guest"));
    }

    //=================================================================================
    //ランキング表示、スコア送信
    //=================================================================================

    /// <summary>
    /// ランキング一覧を表示
    /// </summary>
    public void ShowRanking()
    {
        LobiRankingBridge.PresentRanking();
    }

    /// <summary>
    /// ランキングにスコア送信
    /// </summary>
    public void SendRanking(RankingType rankingType, int score)
    {
        if (LobiCoreBridge.IsSignedIn() && PlayerPrefs.GetString("UserName", "Guest") != "Guest")
        {
            LobiRankingAPIBridge.SendRanking(name, "SendRankingCallback", rankingType.ToString(), score);
        }
        else
        {
            SighUp(PlayerPrefs.GetString("UserName", "Guest"));
        }
    }

    //スコア送信後
    private void SendRankingCallback(string message)
    {
        //ランキング情報を取得し、順位等を確認
        GetRankingData();
    }

    /// <summary>
    /// ランキングデータを取得する
    /// </summary>
    private void GetRankingData()
    {

        foreach (RankingType rankingType in Enum.GetValues(typeof(RankingType)))
        {
            LobiRankingAPIBridge.GetRanking(
              name,
              "GetRankingCallback",
              rankingType.ToString(),
              LobiRankingAPIBridge.RankingRange.All,
              LobiRankingAPIBridge.RankingCursorOrigin.Top,
              1,
              1
            );
        }

    }

    //ランキングデータ取得のコールバック
    private void GetRankingCallback(string message)
    {
        JSONObject json = new JSONObject(message).GetField("result");

        //取得したデータのランキングID
        string rankingID = json.GetField("ranking").GetField("id").str;
        RankingType rankingType = TypeData.KeyToType<RankingType>(rankingID);

        //ランキング参加人数
        string joinCountStr = json.GetField("ranking").GetField("join_count").str;
        _joinCountList[rankingType] = int.Parse(joinCountStr);

        //自分の順位
        string myRankStr = json.GetField("self_order").GetField("rank").str;
        _myRankList[rankingType] = int.Parse(myRankStr);

        //ランキングに登録されている自分のハイスコア
        string highScoreStr = json.GetField("self_order").GetField("score").str;
        _highScoreList[rankingType] = int.Parse(highScoreStr);

        //ランキング情報を取得し、自分の名前が変更されている場合があるので、登録しなおし
        string userName = json.GetField("self_order").GetField("name").str;

        if (!string.IsNullOrEmpty(userName))
        {
            PlayerPrefs.SetString("UserName", userName);
        }

    }

    //=================================================================================
    //ユーザ登録、変更
    //=================================================================================

    //サインアップや名前変更が出来るか
    private bool CanSignUp(string userName)
    {
        //ユーザ名が空
        if (string.IsNullOrEmpty(userName))
        {
            return false;
        }

        PlayerPrefs.SetString("UserName", userName);

        //ネットワークに繋がっていない
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// ユーザ名登録、ログイン
    /// </summary>
    public void SighUp(string userName)
    {
        if (!CanSignUp(userName))
        {
            return;
        }

        LobiCoreAPIBridge.SignupWithBaseName(name, "SignupWithBaseNameCallback", userName);
    }

    //サインアップ完了
    private void SignupWithBaseNameCallback(string message)
    {
        //ユーザ登録が済んだら、デフォルト値をランキングに登録
        foreach (RankingType rankingType in Enum.GetValues(typeof(RankingType)))
        {
            SendRanking(rankingType, RANKING_INITIAL_VALUE[rankingType]);
        }
    }

    /// <summary>
    /// ユーザ名変更
    /// </summary>
    public void ChangeName(string userName)
    {
        if (!CanSignUp(userName))
        {
            return;
        }
        //コールバックを指定しないとiOSは動作しない
        LobiCoreAPIBridge.UpdateUserName(name, "ChangeNameCallback", userName);
    }

    //ユーザ名変更完了
    private void ChangeNameCallback(string message)
    {
    }

}