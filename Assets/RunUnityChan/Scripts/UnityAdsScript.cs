using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsScript : MonoBehaviour {

    void Awake()
    {
#if UNITY_IOS
        Advertisement.Initialize("1067686");
#elif UNITY_ANDROID
        Advertisement.Initialize("1067687");
#endif
    }

    public void ShowAd()
    {
        if(Advertisement.IsReady()) 
        {
            Advertisement.Show();
        }
    }
}
