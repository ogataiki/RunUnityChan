using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ObstacleController : MonoBehaviour
{

    private bool isMoving = true;

    public event Action CollidedWithUnityChan = delegate { };
    public event Action<GameObject> PreDestroy = delegate { };

    [SerializeField]
    public float speed = 0.8f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(this.isMoving)
        {
            Vector3 diff = new Vector3(0.0f, 0.0f, speed) * Time.deltaTime;
            this.gameObject.transform.position = this.gameObject.transform.position - diff;
        }

        if (this.gameObject.transform.position.z <= -10.0f)
        {
            PreDestroy(this.gameObject);
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
        this.CollidedWithUnityChan();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "UnityChan")
        {
            collisionFunc();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "UnityChan")
        {
            collisionFunc();
        }
    }
}
