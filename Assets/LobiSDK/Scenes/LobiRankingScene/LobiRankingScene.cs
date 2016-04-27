using UnityEngine;
using System.Collections;

using Kayac.Lobi.SDK;

public class LobiRankingScene : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Debug.Log("IsSignedIn() = " + (LobiCoreBridge.IsSignedIn() ? "true" : "false"));
		
		// set app link listener
		// LobiChatBridge.SetAppLinkListener(name, "SetAppLinkListenerCallback");		
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(50, 50, 200, 50), "<-")){
			Application.LoadLevel("MainScene");
		}
		if (GUI.Button(new Rect(50, 150, 200, 50), "sendRanking")){
			LobiRankingAPIBridge.SendRanking(name, "SendRankingCallback", "devmassive01", 100); 
		}
		if (GUI.Button(new Rect(50, 250, 200, 50), "getRanking")){
			LobiRankingAPIBridge.GetRanking(name,
			                                "GetRankingCallback",
			                                "devmassive01",
			                                LobiRankingAPIBridge.RankingRange.All,
			                                LobiRankingAPIBridge.RankingCursorOrigin.Top,
			                                1,
			                                10);
		}
		if (GUI.Button(new Rect(50, 350, 200, 50), "getRankingList")){
			LobiRankingAPIBridge.GetRankingList(name,
			                                    "GetRankingListCallback",
			                                    LobiRankingAPIBridge.RankingRange.All);
		}
		if (GUI.Button(new Rect(50, 450, 200, 50), "PresentRanking")){
			LobiRankingBridge.PresentRanking();
		}
	}

	void SendRankingCallback(string message){
		Debug.Log("SendRankingCallback");
		Debug.Log(message);
	}

	void GetRankingCallback(string message){
		Debug.Log("GetRankingCallback");
		Debug.Log(message);
	}

	void GetRankingListCallback(string message){
		Debug.Log("GetRankingListCallback");
		Debug.Log(message);
	}
	
 	void SetAppLinkListenerCallback(string data){
		Debug.Log("get data from lobi chat applink:");
		Debug.Log(data);
	}
}
