using UnityEngine;
using System.Collections;

public class TapEffectController : MonoBehaviour {

    [SerializeField]
    private Camera tapEffectCamera;
    [SerializeField]
    private GameObject tapEffect;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (Time.timeScale <= 0)
        {
            return;
        }

        TouchInfo info = TouchUtil.GetTouch();
        if (info == TouchInfo.Began)
        {
            // タッチ開始
            Debug.Log("TapEffect Began!!");
            Vector3 pos = tapEffectCamera.ScreenToWorldPoint(TouchUtil.GetTouchPosition());
            GameObject effect = (GameObject)Instantiate(tapEffect, new Vector3(pos.x, pos.y, 105.0f), Quaternion.identity);
            Destroy(effect, 0.5f);
        }
        else if (info == TouchInfo.Stationary || info == TouchInfo.Moved)
        {
            // タッチ移動
            Debug.Log("TapEffect Moved!!");
            Vector3 pos = tapEffectCamera.ScreenToWorldPoint(TouchUtil.GetTouchPosition());
            GameObject effect = (GameObject)Instantiate(tapEffect, new Vector3(pos.x, pos.y, 105.0f), Quaternion.identity);
            Destroy(effect, 0.5f);
        }
    }
}
