using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsScript : MonoBehaviour {

    void Awake()
    {
#if UNITY_IPHONE
        Advertisement.Initialize("1067686");
#elif UNITY_ANDROID
        Advertisement.Initialize("1067687");
#endif
    }

    public void ShowAd()
    {
        if(Advertisement.isReady()) 
        {
            Advertisement.Show();
        }
    }
}
