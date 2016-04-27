// Unity4でMultithreaded Renderingを利用する場合、下記のdefineを追加してください 
// #define LOBIREC_MULTITHREADED

#if UNITY_5 && !UNITY_5_0
#define LOBIREC_MULTITHREADED
#endif

#if UNITY_5
#define UNITY_5_AND_LATER
#endif

using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Kayac.Lobi.SDK
{
	public class LobiRec : MonoBehaviour {

		public static bool EnableAudioSettings = true;
		private static bool CanCallNativeMethods = false;

		const int EVENT_ID_INIT_CAPTURE = 1;
		const int EVENT_ID_ON_END_OF_FRAME = 2;
		const int EVENT_ID_CAMERA_CAPTURE_AND_RENDER = 3;
		const int EVENT_ID_CAMERA_PRE_RENDER = 4;
		const int EVENT_ID_ON_RESUME_ACTIVITY = 5;
		const int EVENT_ID_ON_PAUSE_ACTIVITY = 6;

		#if UNITY_ANDROID && !UNITY_EDITOR && UNITY_5_AND_LATER
		private static int initDelayFrames = 10;
		#else
		private static int initDelayFrames = 0;
		#endif

		public static void SetUnityVersion(string version) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecUnity");
			jc.CallStatic("setUnityVersion", version);
			#else
			Debug.Log("unsupported");
			#endif
		}

		public static void SetResolution(int w, int h) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecUnity");
			jc.CallStatic("setResolution", w, h);
			#else
			Debug.Log("unsupported");
			#endif
		}

		public static void SetAudioFormat(int sampleRate, int channelCount) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecUnity");
			jc.CallStatic("setAudioFormat", sampleRate, channelCount);
			#else
			Debug.Log("unsupported");
			#endif
		}

		#if UNITY_ANDROID || ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
		private static LobiRec instance = null;

		void Awake() {
			if(instance != null) {
				Destroy(this);
				return;
			}

			#if UNITY_ANDROID && ! UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.kayac.lobi.sdk.rec.unity.LobiRecUnity");
			CanCallNativeMethods = jc.CallStatic<bool>("loadNativeLibrary");
			if (CanCallNativeMethods) {
				new AndroidJavaClass("com.kayac.lobi.sdk.rec.externalaudio.ExternalAudioInput");
			}
			#elif (UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR
			CanCallNativeMethods = true;
			#endif

			if (CanCallNativeMethods) {
				DontDestroyOnLoad(this.gameObject);
				instance = this;
				if (initDelayFrames == 0) {
					InitLobiRec();
				}
				AutoRecord();
			} else {
				Debug.Log("disabled");
			}
		}

		void OnLevelWasLoaded(int level) {
			AutoRecord();
		}

		private static LobiRec Instance {
			get {
				return instance;
			}
		}

		void OnApplicationPause(bool pauseStatus) {
			if (pauseStatus) {
				OnPauseActivity();
			} else {
				OnResumeActivity();
			}
		}

		void AutoRecord() {
			// add LobiRecCamera
			Camera[] cameraList = (Camera[]) FindObjectsOfType (typeof(Camera));
			int attached = 0;
			foreach (Camera camera in cameraList) {
				if (camera.gameObject.GetComponent<LobiRecCamera>() != null) {
					attached++;
				}
			}
			if (attached == 0) {
				foreach (Camera camera in cameraList) {
					if (camera.targetTexture == null) {
						camera.gameObject.AddComponent<LobiRecCamera>();
					}
				}
			}
		}
		
		IEnumerator RecordRenderBuffer() {
			while(true) {
				if (initDelayFrames > 0 && --initDelayFrames == 0) {
					InitLobiRec();
				}
				yield return new WaitForEndOfFrame();
				OnEndOfFrame();
			}
		}
		#endif

		#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
		void Start() {
			#if !UNITY_5_AND_LATER
			if (EnableAudioSettings == true) {
				AudioSettings.outputSampleRate = 44100;
			} 
			#endif
			StartCoroutine(RecordRenderBuffer());
		}
		#elif UNITY_ANDROID
		void Start() {
			if (!CanCallNativeMethods) {
				return;
			}
			StartCoroutine(RecordRenderBuffer());
		}
		#endif

		public static void InitLobiRec() {
			if (!CanCallNativeMethods) {
				return;
			}
			LoadNativePlugin();

			SetAudioFormat(AudioSettings.outputSampleRate, 2);

			#if UNITY_ANDROID && !UNITY_EDITOR && UNITY_4_2
			Debug.Log("InitLobiRec:version4.2");
			SetUnityVersion("4.2");
			#elif UNITY_ANDROID && !UNITY_EDITOR && UNITY_4_3
			Debug.Log("InitLobiRec:version4.3");
			SetUnityVersion("4.3");
			#elif UNITY_ANDROID && !UNITY_EDITOR && UNITY_4_5
			Debug.Log("InitLobiRec:version4.5");
			SetUnityVersion("4.5");
			#elif UNITY_ANDROID && !UNITY_EDITOR && (UNITY_4_6 || UNITY_4_7)
			Debug.Log("InitLobiRec:version4.6");
			SetUnityVersion("4.6");
			#elif UNITY_ANDROID && !UNITY_EDITOR && UNITY_5_0
			Debug.Log("InitLobiRec:version5.0");
			SetUnityVersion("5.0");
			#elif UNITY_ANDROID && !UNITY_EDITOR && UNITY_5_1
			Debug.Log("InitLobiRec:version5.1");
			SetUnityVersion("5.1");
			#elif UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("InitLobiRec:version5.2");
			SetUnityVersion("5.2");
			#endif
			#if UNITY_ANDROID && !UNITY_EDITOR
			#if LOBIREC_MULTITHREADED
			Debug.Log("InitLobiRec:issue plugin event");
			#else
			Debug.Log("InitLobiRec:unity render event");
			#endif
			CallUnityRenderEvent(EVENT_ID_INIT_CAPTURE);
			#elif ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			initLobiRec_();
			#endif
		}

		public static void CameraPreRender() {
			if (!CanCallNativeMethods) {
				return;
			}
			#if UNITY_ANDROID && !UNITY_EDITOR
			CallUnityRenderEvent(EVENT_ID_CAMERA_PRE_RENDER);
			#elif ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			cameraPreRender_();
			#endif
		}

		public static void CameraCaptureAndRender() {
			if (!CanCallNativeMethods) {
				return;
			}
			#if UNITY_ANDROID && !UNITY_EDITOR
			CallUnityRenderEvent(EVENT_ID_CAMERA_CAPTURE_AND_RENDER);
			#endif
		}
		
		public static void OnEndOfFrame() {
			if (!CanCallNativeMethods) {
				return;
			}
			#if UNITY_ANDROID && !UNITY_EDITOR
			CallUnityRenderEvent(EVENT_ID_ON_END_OF_FRAME);
			#elif ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			onEndOfFrame_();
			#endif
		}

		public static void OnResumeActivity() {
			if (!CanCallNativeMethods) {
				return;
			}
			#if UNITY_ANDROID && !UNITY_EDITOR
			CallUnityRenderEvent(EVENT_ID_ON_RESUME_ACTIVITY);
			#elif ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_resume_();
			#endif
		}

		public static void OnPauseActivity() {
			if (!CanCallNativeMethods) {
				return;
			}
			#if UNITY_ANDROID && !UNITY_EDITOR
			CallUnityRenderEvent(EVENT_ID_ON_PAUSE_ACTIVITY);
			#elif ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
			LobiRec_pause_();
			#endif
		}

		#if ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
		[DllImport("__Internal")]
		private static extern void cameraPreRender_();

		[DllImport("__Internal")]
		private static extern void onEndOfFrame_();
		
		[DllImport("__Internal")]
		private static extern void initLobiRec_();

		[DllImport("__Internal")]
		private static extern void LobiRec_pause_();

		[DllImport("__Internal")]
		private static extern void LobiRec_resume_();
		#endif
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		[DllImport("lobirecunity")]
		private static extern void LoadNativePlugin();
		[DllImport("lobirecunity")]
		private static extern void UnityRenderEvent(int eventId);
		// Check if lobirecunity was loaded,
		// before calling this.
		private static void CallUnityRenderEvent(int eventId) {
			#if LOBIREC_MULTITHREADED
			GL.IssuePluginEvent(eventId);
			#else
			UnityRenderEvent(eventId);
			#endif
		}
		#else
		private static void LoadNativePlugin() {
			// do nothing.
		}
		#endif
	}
}
