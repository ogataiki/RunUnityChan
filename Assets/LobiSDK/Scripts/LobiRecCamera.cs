using UnityEngine;

namespace Kayac.Lobi.SDK
{
	public class LobiRecCamera : MonoBehaviour {
		#if UNITY_ANDROID || ((UNITY_IOS || UNITY_IPHONE) && ! UNITY_EDITOR)
		void OnPreRender() {
			LobiRec.CameraPreRender();
		}
		#endif
	}
}
