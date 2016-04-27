#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
#define UNITY_4_X
#endif

using UnityEngine;
using System.Collections;

using Kayac.Lobi.SDK;

public class MainScene : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Debug.Log("IsSignedIn() = " + (LobiCoreBridge.IsSignedIn() ? "true" : "false"));
		// Lobi Rec SDK を使用する際は起動時に以下の設定を必ず行ってください。
		#if (UNITY_IOS || UNITY_IPHONE) && UNITY_4_X
		AudioSettings.outputSampleRate = 44100;
		#endif
		
		// set app link listener
		// LobiChatBridge.SetAppLinkListener(name, "SetAppLinkListenerCallback");
		
		LobiCoreBridge.SetupPopOverController(100, 100, LobiCoreBridge.PopOverArrowDirection.Left);
		// Use for customize
		// LobiCoreBridge.SetNavigationBarCustomColor(1.0f, 0.5f, 0.0f);

		LobiEventReceiver.Instance.DissmissedAction = () => {
			Debug.Log ("DissmissedAction");
		};
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(50, 50, 200, 50), "LobiCore")){
			Application.LoadLevel("LobiCoreScene");
		}
		if (GUI.Button(new Rect(50, 150, 200, 50), "LobiRec")){
			Application.LoadLevel("LobiRecScene");
		}
		if (GUI.Button(new Rect(50, 250, 200, 50), "LobiChat")){
			Application.LoadLevel("LobiChatScene");
		}
		if (GUI.Button(new Rect(50, 350, 200, 50), "LobiRanking")){
			Application.LoadLevel("LobiRankingScene");
		}
	}

 	void SetAppLinkListenerCallback(string data){
		Debug.Log("get data from lobi chat applink:");
		Debug.Log(data);
	}
}

