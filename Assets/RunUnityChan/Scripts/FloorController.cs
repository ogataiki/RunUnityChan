using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FloorController : MonoBehaviour {

    private bool isMoving = true;

    public event Action<GameObject> PreDestroy = delegate { };

    [SerializeField]
    public float speed = 0.8f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.isMoving)
        {
            Vector3 diff = new Vector3(0.0f, 0.0f, speed) * Time.deltaTime;
            this.gameObject.transform.position = this.gameObject.transform.position - diff;
        }

        if (this.gameObject.transform.position.z <= -10.0f)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 3.0f);
        }
    }

    public void GoTitle()
    {
        PreDestroy(this.gameObject);
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void Stop(bool value)
    {
        isMoving = (value) ? false : true;
    }

    void collisionFunc()
    {
        this.isMoving = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("UnityChan"))
        {
            collisionFunc();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Contains("UnityChan"))
        {
            collisionFunc();
        }
    }
}
