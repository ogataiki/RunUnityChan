using UnityEngine;
using System.Collections;

using Kayac.Lobi.SDK;

public class LobiRecScene : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Debug.Log("IsSignedIn() = " + (LobiCoreBridge.IsSignedIn() ? "true" : "false"));
		LobiRecBridge.RegisterMicEnableErrorObserver(
			name,
			"MicEnableErrorCallback"
		);
		LobiRecBridge.RegisterDismissingPostVideoViewNotification(
			name,
			"DismissingPostVideoViewCallback"
		);
		
		// set app link listener
		// LobiChatBridge.SetAppLinkListener(name, "SetAppLinkListenerCallback");
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(50, 50, 200, 50), "<-")){
			LobiRecBridge.StopCapturing();
			Application.LoadLevel("MainScene");
		}
		if (GUI.Button(new Rect(50, 150, 200, 50), "StartCapturing")){
			LobiRecBridge.SetMicEnable(true);
			LobiRecBridge.SetMicVolume(1.0f);
			LobiRecBridge.SetGameSoundVolume(0.2f);

			if (LobiRecBridge.IsFaceCaptureSupported()) {
				LobiRecBridge.SetLiveWipeStatus(LobiRecBridge.LiveWipeStatus.InCamera);
				LobiRecBridge.SetWipePositionX(100.0f);
				LobiRecBridge.SetWipePositionY(100.0f);
				LobiRecBridge.SetWipeSquareSize(100.0f); 
			}
			
			LobiRecBridge.SetPreventSpoiler(false);
			LobiRecBridge.SetCapturePerFrame(2);
			LobiRecBridge.StartCapturing();
		}
		if (GUI.Button(new Rect(50, 250, 200, 50), "StopCapturing")){
			#if UNITY_ANDROID && !UNITY_EDITOR
			LobiRecBridge.StopCapturing();			
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRecBridge.StopCapturing();			
			// LobiRecBridge.StopCapturingWithCallback(
			// 	name,
			// 	"StopCapturingCallback");
			#endif
		}
		if (GUI.Button(new Rect(50, 350, 200, 50), "PresentLobiPost")){
			LobiRecBridge.PresentLobiPost("sample title", "sample description", 0, "", "{\"type\":\"sample video\",\"game_engine\":\"unity\"}");
		}
		if (GUI.Button(new Rect(50, 450, 200, 50), "PresentLobiPlay")){
			LobiRecBridge.PresentLobiPlay();
			// LobiRecBridge.PresentLobiPlay("xxxxxxx");
			// LobiRecBridge.PresentLobiPlay("", "", false, "{\"game_engine\":\"unity\"}");
			// LobiRecBridge.PresentLobiPlayWithEventFields("event_type1:1");
		}
		if (GUI.Button(new Rect(50, 550, 200, 50), "Snap")){
			LobiRecBridge.Snap(
				name,
				"SnapCallback"
			); 
		}
		if (GUI.Button(new Rect(50, 650, 200, 50), "SnapFace")){
			LobiRecBridge.SnapFace(
				name,
				"SnapFaceCallback"
			); 
		}
		if (GUI.Button(new Rect(50, 750, 200, 50), "IsMicEnabled")){
			LobiRecBridge.IsMicEnabled(
				name,
				"IsMicEnabledCallback"
			);
		}
		if (GUI.Button(new Rect(50, 850, 200, 50), "IsSupported")){
			Debug.Log("IsSupported() = " + (LobiRecBridge.IsSupported() ? "true" : "false"));
		}
		if (GUI.Button(new Rect(50, 950, 200, 50), "Play BGM")){
			GameObject.Find("BGM Player").GetComponent<AudioSource>().Play();
		}
		if (GUI.Button(new Rect(50, 1050, 200, 50), "PauseCapturing")){
			LobiRecBridge.PauseCapturing ();
        }
        
		if (GUI.Button(new Rect(50, 1150, 200, 50), "ResumeCapturing")){
			LobiRecBridge.ResumeCapturing ();
        }
        
		if (GUI.Button(new Rect(50, 1250, 200, 50), "IsPaused")){
			Debug.Log("IsPaused() = " + (LobiRecBridge.IsPaused() ? "true" : "false"));
        }
	}
 
 	void StopCapturingCallback(string message)
 	{
		Debug.Log("StopCapturingCallback");
		Debug.Log(message);
 	}

	void SnapCallback(string message)
	{
		Debug.Log("SnapCallback");
		Debug.Log(message);
	}

	void SnapFaceCallback(string message)
	{
		Debug.Log("SnapFaceCallback");
		Debug.Log(message);
	}

	void IsMicEnabledCallback(string message)
	{
		Debug.Log("IsMicEnabledCallback");
		Debug.Log(message);
	}

	void MicEnableErrorCallback(string message)
	{
		Debug.Log("micEnableError");
	}
	
	void DismissingPostVideoViewCallback(string message)
	{
		Debug.Log(message);
	}

	void SetAppLinkListenerCallback(string data){
		Debug.Log("get data from lobi chat applink:");
		Debug.Log(data);
	}
}
