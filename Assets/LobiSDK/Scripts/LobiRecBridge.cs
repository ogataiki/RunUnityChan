using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Kayac.Lobi.SDK
{
	public class LobiRecBridge : object
	{
		public static ILobiRecDelegates LobiRecDelegates;

		public enum LiveWipeStatus
		{
			None = 0,
			InCamera,
			Icon,
		}

		public static bool IsCapturing(){
			bool isCapturing = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			isCapturing = lobiClass.CallStatic<bool>("isCapturing");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			isCapturing = LobiRec_is_capturing_() == 1;
			#endif
			return isCapturing;
		}

		public static bool IsFaceCaptureSupported(){
			bool isFaceCaptureSupported = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			#if UNITY_5
			isFaceCaptureSupported = false;
			#else
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			isFaceCaptureSupported = lobiClass.CallStatic<bool>("isFaceCaptureSupported");
			#endif
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			isFaceCaptureSupported = true;
			#endif
			return isFaceCaptureSupported;
		}

		public static void SetLiveWipeStatus(LiveWipeStatus status){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setLiveWipeStatus", (int)status);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_live_wipe_status_((int)status);
			#endif
		}

		[Obsolete("This method is obsolete; use method GetLiveWipeStatus instead")]
		public static LiveWipeStatus SetLiveWipeStatus(){
			LiveWipeStatus status = LiveWipeStatus.None;
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			status = (LiveWipeStatus)LobiRec_get_live_wipe_status_();
			#endif
			return status;
		}

		public static LiveWipeStatus GetLiveWipeStatus(){
			LiveWipeStatus status = LiveWipeStatus.None;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			status = (LiveWipeStatus)lobiClass.CallStatic<int>("getLiveWipeStatus");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			status = (LiveWipeStatus)LobiRec_get_live_wipe_status_();
			#endif
			return status;
		}

		public static void SetWipePositionX(float x){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setWipePositionX", x);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_wipe_position_x_(x);
			#endif
		}

		public static float GetWipePositionX(){
			float x = 0.0f;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			x = lobiClass.CallStatic<float>("getWipePositionX");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			x = LobiRec_get_wipe_position_x_();
			#endif
			return x;
		}

		public static void SetWipePositionY(float y){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setWipePositionY", y);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_wipe_position_y_(y);
			#endif
		}

		public static float GetWipePositionY(){
			float y = 0.0f;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			y = lobiClass.CallStatic<float>("getWipePositionY");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			y = LobiRec_get_wipe_position_y_();
			#endif
			return y;
		}

		public static void SetWipeSquareSize(float size){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setWipeSquareSize", size);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_wipe_square_size_(size);
			#endif
		}

		public static float GetWipeSquareSize(){
			float size = 0.0f;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			size = lobiClass.CallStatic<float>("getWipeSquareSize");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			size = LobiRec_get_wipe_square_size_();
			#endif
			return size;
		}

		public static void SetGameSoundVolume(float volume){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setGameSoundVolume", volume);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_game_sound_volume_(volume);
			#endif
		}

		public static float GetGameSoundVolume(){
			float volume = 0.0f;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			volume = lobiClass.CallStatic<float>("getGameSoundVolume");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			volume = LobiRec_get_game_sound_volume_();
			#endif
			return volume;
		}

		public static void SetAfterRecordingVolume(float volume){
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_after_recording_volume_(volume);
			#endif
		}

		public static float GetAfterRecordingVolume(){
			float volume = 0.0f;
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			volume = LobiRec_get_after_recording_volume_();
			#endif
			return volume;
		}

		public static void SetMicVolume(float volume){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setMicVolume", volume);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_mic_volume_(volume);
			#endif
		}

		public static float GetMicVolume(){
			float volume = 0.0f;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			volume = lobiClass.CallStatic<float>("getMicVolume");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			volume = LobiRec_get_mic_volume_();
			#endif
			return volume;
		}

		public static void SetMicEnable(bool enable){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setMicEnable", enable);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_mic_enable_(enable ? 1 : 0);
			#endif
		}

		public static bool GetMicEnable(){
			bool enable = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			enable = lobiClass.CallStatic<bool>("getMicEnable");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			enable = LobiRec_get_mic_enable_() == 1;
			#endif
			return enable;
		}

		public static void SetPreventSpoiler(bool enable){
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_prevent_spoiler_(enable ? 1 : 0);
			#endif
		}

		public static bool GetPreventSpoiler(){
			bool enable = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			enable = LobiRec_get_prevent_spoiler_() == 1;
			#endif
			return enable;
		}

		public static void SetHideFaceOnPreview(bool enable){
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_hide_face_on_preview_(enable ? 1 : 0);
			#endif
		}

		public static bool GetHideFaceOnPreview(){
			bool enable = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			enable = LobiRec_get_hide_face_on_preview_() == 1;
			#endif
			return enable;
		}

		public static void SetCapturePerFrame(int frame){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setCapturePerFrame", frame);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_capture_per_frame_(frame);
			#endif
		}

		public static int GetCapturePerFrame(){
			int frame = 0;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			frame = lobiClass.CallStatic<int>("getCapturePerFrame");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			frame = LobiRec_get_capture_per_frame_();
			#endif
			return frame;
		}
		
		public static void SetStickyRecording(bool enable){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("setStickyRecording", enable);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_set_sticky_recording_(enable ? 1 : 0);
			#endif
		}
		
		public static bool GetStickyRecording(){
			bool enable = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			enable = lobiClass.CallStatic<bool>("getStickyRecording");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			enable = LobiRec_get_sticky_recording_() == 1;
			#endif
			return enable;
		}

		public static void StartCapturing(){
			if (LobiRecDelegates != null) {
				LobiRecDelegates.BeforeStartCapturingDelegate();
			}

			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("startCapturing");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_start_capturing_();
			#endif

			if (LobiRecDelegates != null) {
				LobiRecDelegates.AfterStartCapturingDelegate();
			}
		}

		public static void StopCapturingWithCallback(string gameObjectName, string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			return;
			#endif
			if (LobiRecDelegates != null) {
				LobiRecDelegates.BeforeStartCapturingDelegate();
			}
			
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName     = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_stop_capturing_with_callback_(
				cGameObjectName, 
				cGameObjectName.Length,
				cCallbackMethodName, 
				cCallbackMethodName.Length
			);
			#endif

			if (LobiRecDelegates != null) {
				LobiRecDelegates.AfterStartCapturingDelegate();
			}
		}

		public static void StopCapturing(){
			if (LobiRecDelegates != null) {
				LobiRecDelegates.BeforeStopCapturingDelegate();
			}

			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("stopCapturing");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_stop_capturing_();
			#endif

			if (LobiRecDelegates != null) {
				LobiRecDelegates.AfterStopCapturingDelegate();
			}
		}

		public static void ResumeCapturing(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("resumeCapturing");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_resume_capturing_();
			#endif
		}

		public static void PauseCapturing(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("pauseCapturing");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_pause_capturing_();
			#endif
		}

		public static bool IsPaused(){
			bool isPaused = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			isPaused = lobiClass.CallStatic<bool>("isPaused");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			isPaused = LobiRec_is_paused_() == 1;
			#endif
			return isPaused;
		}

		public static bool HasMovie(){
			bool hasMovie = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			hasMovie = lobiClass.CallStatic<bool>("hasMovie");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			hasMovie = LobiRec_has_movie_() == 1;
			#endif
			return hasMovie;
		}

		public static bool IsSupported(){
			bool supported = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			supported = lobiClass.CallStatic<bool>("isSupported");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			supported = true;
			#endif
			return supported;			
		}

		public static void Snap(string gameObjectName, string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName     = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_snap_(
				cGameObjectName, 
				cGameObjectName.Length,
				cCallbackMethodName, 
				cCallbackMethodName.Length
			);
			#endif
		}

		public static void SnapFace(string gameObjectName, string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("unsupported");
			#endif
			
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName     = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_snap_face_(
				cGameObjectName, 
				cGameObjectName.Length,
				cCallbackMethodName, 
				cCallbackMethodName.Length
			);
			#endif
		}

		public static void IsMicEnabled(string gameObjectName, string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("isMicEnabled", gameObjectName, callbackMethodName);
			#endif
			
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName     = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_is_mic_enabled_(
				cGameObjectName, 
				cGameObjectName.Length,
				cCallbackMethodName, 
				cCallbackMethodName.Length
			);
			#endif
		}

		public static void PresentLobiPlay(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("openLobiPlayActivity");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_present_lobi_play_();
			#endif
		}

		public static void PresentLobiPlay(string userExid, string category, bool letsplay, string metaJson){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("openLobiPlayActivity", userExid, category, letsplay, metaJson);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cUserExId           = System.Text.Encoding.UTF8.GetBytes(userExid);
			byte[] cCategory 		   = System.Text.Encoding.UTF8.GetBytes(category);
			int    cLetsPlay 		   = letsplay ? 1 : 0;
			byte[] cMetaFields    	   = System.Text.Encoding.UTF8.GetBytes(metaJson);
			LobiRec_present_lobi_play_with_conditions_(cUserExId, cUserExId.Length,
														cCategory, cCategory.Length,
														cLetsPlay,
														cMetaFields, cMetaFields.Length);
			#endif
		}

		public static void PresentLobiPlayWithEventFields(string eventFields){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("openLobiPlayActivityWithEventFields", eventFields);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cEventFields = System.Text.Encoding.UTF8.GetBytes(eventFields);
			LobiRec_present_lobi_play_with_event_fields_(cEventFields, cEventFields.Length);
			#endif
		}

		public static void PresentLobiPlay(string videoId){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("openLobiPlayActivity", videoId);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cVideoId = System.Text.Encoding.UTF8.GetBytes(videoId);
			LobiRec_present_lobi_play_with_video_id_(cVideoId, cVideoId.Length);
			#endif
		}

		public static void PresentLobiPost(string title,
		                                   string postDescription,
		                                   System.Int64 postScore,
		                                   string postCategory){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("openPostVideoActivity", title, postDescription, postScore, postCategory);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cTitle           = System.Text.Encoding.UTF8.GetBytes(title);
			byte[] cPostDescription = System.Text.Encoding.UTF8.GetBytes(postDescription);
			byte[] cPostCategory    = System.Text.Encoding.UTF8.GetBytes(postCategory);
			byte[] cPostMetaData    = System.Text.Encoding.UTF8.GetBytes("");

			LobiRec_present_lobi_post_(cTitle, cTitle.Length,
										cPostDescription, cPostDescription.Length,
										postScore,
										cPostCategory, cPostCategory.Length,
										cPostMetaData, cPostMetaData.Length);
			#endif
		}

		public static void PresentLobiPost(string title,
		                                    string postDescription,
		                                    System.Int64 postScore,
		                                    string postCategory,
		                                    string postMetadata){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("openPostVideoActivity", title, postDescription, postScore, postCategory, postMetadata);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cTitle           = System.Text.Encoding.UTF8.GetBytes(title);
			byte[] cPostDescription = System.Text.Encoding.UTF8.GetBytes(postDescription);
			byte[] cPostCategory    = System.Text.Encoding.UTF8.GetBytes(postCategory);
			byte[] cPostMetaData    = System.Text.Encoding.UTF8.GetBytes(postMetadata);

			LobiRec_present_lobi_post_(cTitle, cTitle.Length,
			                           cPostDescription, cPostDescription.Length,
			                           postScore,
			                           cPostCategory, cPostCategory.Length,
									   cPostMetaData, cPostMetaData.Length);
			#endif
		}
		
		public static void RegisterMicEnableErrorObserver(string gameObjectName,
		                                                   string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerMicEnableErrorObserver", gameObjectName, callbackMethodName);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName      = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName  = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_mic_enable_error_observer_(
				cGameObjectName, 
				cGameObjectName.Length,
				cCallbackMethodName, 
				cCallbackMethodName.Length
			);
			#endif
		}

		public static void UnregisterMicEnableErrorObserver(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("unregisterMicEnableErrorObserver");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_unregister_mic_enable_error_observer_();
			#endif
		}

		public static void RegisterDryingUpInStorageObserver(string gameObjectName,
		                                                      string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerDryingUpInStorageObserver", gameObjectName, callbackMethodName);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName      = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName  = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_register_drying_up_in_storage_observer_(cGameObjectName, cGameObjectName.Length,
			                                                cCallbackMethodName, cCallbackMethodName.Length);
			#endif
		}

		public static void UnregisterDryingUpInStorageObserver(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("unregisterDryingUpInStorageObserver");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_unregister_drying_up_in_storage_observer_();
			#endif
		}

		public static void RegisterMovieCreatedNotification(string gameObjectName,
		                                                     string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerMovieCreatedNotification", gameObjectName, callbackMethodName);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName      = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName  = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_register_movie_created_notification_(cGameObjectName, cGameObjectName.Length,
			                                             cCallbackMethodName, cCallbackMethodName.Length);
			#endif
		}

		public static void UnregisterMovieCreatedNotification(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("unregisterMovieCreatedNotification");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_unregister_movie_created_notification_();
			#endif
		}

		public static void RegisterMovieCreatedErrorNotification(string gameObjectName,
		                                                          string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerMovieCreatedErrorNotification", gameObjectName, callbackMethodName);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName      = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName  = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_register_movie_created_error_notification_(cGameObjectName, cGameObjectName.Length,
			                                                   cCallbackMethodName, cCallbackMethodName.Length);
			#endif
		}

		public static void UnregisterMovieCreatedErrorNotification(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("unregisterMovieCreatedErrorNotification");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_unregister_movie_created_error_notification_();
			#endif
		}

		public static void RegisterMovieUploadedNotification(string gameObjectName,
		                                                      string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerMovieUploadedNotification", gameObjectName, callbackMethodName);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName      = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName  = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_register_movie_uploaded_notification_(cGameObjectName, cGameObjectName.Length,
			                                              cCallbackMethodName, cCallbackMethodName.Length);
			#endif
		}

		public static void UnregisterMovieUploadedNotification(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerMovieUploadedNotification");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_unregister_movie_uploaded_notification_();
			#endif
		}

		public static void RegisterMovieUploadedErrorNotification(string gameObjectName,
		                                                           string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerMovieUploadedErrorNotification", gameObjectName, callbackMethodName);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName      = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName  = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_register_movie_uploaded_error_notification_(cGameObjectName, cGameObjectName.Length,
			                                                    cCallbackMethodName, cCallbackMethodName.Length);
			#endif
		}

		public static void UnregisterMovieUploadedErrorNotification(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("unregisterMovieUploadedErrorNotification");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_unregister_movie_uploaded_error_notification_();
			#endif
		}

		public static void RegisterDismissingPostVideoViewNotification(string gameObjectName,
		                                                                string callbackMethodName){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("registerDismissingPostVideoViewNotification", gameObjectName, callbackMethodName);
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			byte[] cGameObjectName      = System.Text.Encoding.UTF8.GetBytes(gameObjectName);
			byte[] cCallbackMethodName  = System.Text.Encoding.UTF8.GetBytes(callbackMethodName);
			LobiRec_register_dismissing_post_video_view_controller_notification_(cGameObjectName, cGameObjectName.Length,
			                                                                     cCallbackMethodName, cCallbackMethodName.Length);
			#endif
		}

		public static void UnregisterDismissingPostVideoViewNotification(){
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			lobiClass.CallStatic("unregisterDismissingPostVideoViewNotification");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_unregister_dismissing_post_video_view_controller_notification_();
			#endif
		}

		public static bool RemoveUnretainedVideo(){
			bool result = false;
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass lobiClass = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecBridge");
			result = lobiClass.CallStatic<bool>("removeUnretainedVideo");
			#endif
			#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			result = LobiRec_remove_unretained_video_() == 1;
			#endif
			return result;
		}

		#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
		[DllImport("__Internal")]
		private static extern int LobiRec_set_sticky_recording_(int enable);

		[DllImport("__Internal")]
		private static extern int LobiRec_get_sticky_recording_();

		[DllImport("__Internal")]
		private static extern int LobiRec_is_capturing_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_live_wipe_status_(int status);

		[DllImport("__Internal")]
		private static extern int LobiRec_get_live_wipe_status_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_wipe_position_x_(float x);

		[DllImport("__Internal")]
		private static extern float LobiRec_get_wipe_position_x_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_wipe_position_y_(float y);
		
		[DllImport("__Internal")]
		private static extern float LobiRec_get_wipe_position_y_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_wipe_square_size_(float size);

		[DllImport("__Internal")]
		private static extern float LobiRec_get_wipe_square_size_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_game_sound_volume_(float volume);

		[DllImport("__Internal")]
		private static extern float LobiRec_get_game_sound_volume_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_after_recording_volume_(float volume);

		[DllImport("__Internal")]
		private static extern float LobiRec_get_after_recording_volume_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_mic_volume_(float volume);
		
		[DllImport("__Internal")]
		private static extern float LobiRec_get_mic_volume_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_mic_enable_(int enable);
		
		[DllImport("__Internal")]
		private static extern int LobiRec_get_mic_enable_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_prevent_spoiler_(int enable);
		
		[DllImport("__Internal")]
		private static extern int LobiRec_get_prevent_spoiler_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_hide_face_on_preview_(int enable);
		
		[DllImport("__Internal")]
		private static extern int LobiRec_get_hide_face_on_preview_();

		[DllImport("__Internal")]
		private static extern void LobiRec_set_capture_per_frame_(int frame);
		
		[DllImport("__Internal")]
		private static extern int LobiRec_get_capture_per_frame_();

		[DllImport("__Internal")]
		private static extern void LobiRec_start_capturing_();

		[DllImport("__Internal")]
		private static extern void LobiRec_stop_capturing_();

		[DllImport("__Internal")]
		private static extern void LobiRec_resume_capturing_();

		[DllImport("__Internal")]
		private static extern void LobiRec_pause_capturing_();

		[DllImport("__Internal")]
		private static extern int LobiRec_is_paused_();

		[DllImport("__Internal")]
		private static extern int LobiRec_has_movie_();

		[DllImport("__Internal")]
		private static extern void LobiRec_present_lobi_play_();

		[DllImport("__Internal")]
		private static extern void LobiRec_present_lobi_play_with_video_id_(
			byte[] video_id, int video_id_len
		);

		[DllImport("__Internal")]
		private static extern void LobiRec_present_lobi_play_with_conditions_(
			byte[] play_user_exid, int play_user_exid_len,
			byte[] play_category, int play_category_len,
			int play_lets_play,
			byte[] play_meta_fields, int play_meta_fields_len
		);

		[DllImport("__Internal")]
		private static extern void LobiRec_present_lobi_play_with_event_fields_(
			byte[] event_fields, int event_fields_len
		);

		[DllImport("__Internal")]
		private static extern void LobiRec_snap_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len
		);

		[DllImport("__Internal")]
		private static extern void LobiRec_snap_face_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len
		);

		[DllImport("__Internal")]
		private static extern void LobiRec_is_mic_enabled_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len
		);
		
		[DllImport("__Internal")]
		private static extern void LobiRec_stop_capturing_with_callback_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len
		);

		[DllImport("__Internal")]
		private static extern void LobiRec_present_lobi_post_(
			byte[] title, int title_len,
			byte[] post_description, int post_description_len,
			System.Int64 post_score,
			byte[] post_category, int post_category_len,
			byte[] post_metadata, int post_metadata_len);

		[DllImport("__Internal")]
		private static extern void LobiRec_mic_enable_error_observer_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len);

		[DllImport("__Internal")]
		private static extern void LobiRec_unregister_mic_enable_error_observer_();
		
		[DllImport("__Internal")]
		private static extern void LobiRec_register_drying_up_in_storage_observer_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len);

		[DllImport("__Internal")]
		private static extern void LobiRec_unregister_drying_up_in_storage_observer_();

		[DllImport("__Internal")]
		private static extern void LobiRec_register_movie_created_notification_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len);
		
		[DllImport("__Internal")]
		private static extern void LobiRec_unregister_movie_created_notification_();

		[DllImport("__Internal")]
		private static extern void LobiRec_register_movie_created_error_notification_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len);
		
		[DllImport("__Internal")]
		private static extern void LobiRec_unregister_movie_created_error_notification_();

		[DllImport("__Internal")]
		private static extern void LobiRec_register_movie_uploaded_notification_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len);
		
		[DllImport("__Internal")]
		private static extern void LobiRec_unregister_movie_uploaded_notification_();

		[DllImport("__Internal")]
		private static extern void LobiRec_register_movie_uploaded_error_notification_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len);
		
		[DllImport("__Internal")]
		private static extern void LobiRec_unregister_movie_uploaded_error_notification_();

		[DllImport("__Internal")]
		private static extern void LobiRec_register_dismissing_post_video_view_controller_notification_(
			byte[] game_object_name, int game_object_name_len,
			byte[] callback_method_name, int callback_method_name_len);
		
		[DllImport("__Internal")]
		private static extern void LobiRec_unregister_dismissing_post_video_view_controller_notification_();

		[DllImport("__Internal")]
		private static extern int LobiRec_remove_unretained_video_();
		#endif
	}
}
