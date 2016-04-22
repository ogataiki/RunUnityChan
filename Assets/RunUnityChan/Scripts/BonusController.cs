using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BonusController : MonoBehaviour {


    private bool isMoving = true;

    public event Action<Vector3> CollidedWithUnityChan = delegate { };
    public event Action ThroughedWithUnityChan = delegate { };
    public event Action<GameObject> PreDestroy = delegate { };

    [SerializeField]
    public float speed = 1.0f;

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

        if (this.gameObject.transform.position.z <= -1.5f)
        {
            this.isMoving = false;
            this.PreDestroy(this.gameObject);
            this.ThroughedWithUnityChan();
        }
    }

    public void GoTitle()
    {
        PreDestroy(this.gameObject);
    }

    public void Stop(bool value)
    {
        isMoving = (value) ? false : true;
    }

    void collisionFunc()
    {
        this.isMoving = false;
        this.PreDestroy(this.gameObject);
        this.CollidedWithUnityChan(this.gameObject.transform.position);
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
