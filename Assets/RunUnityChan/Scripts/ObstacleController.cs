using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ObstacleController : MonoBehaviour
{

    private bool isMoving = true;

    public event Action CollidedWithUnityChan = delegate { };
    public event Action<GameObject> PreDestroy = delegate { };

    [SerializeField]
    public float speed = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(this.isMoving)
        {
            Vector3 diff = new Vector3(0.0f, 0.0f, speed) * Time.deltaTime;
            this.gameObject.transform.position = Vector3.Lerp(
                this.gameObject.transform.position,
                this.gameObject.transform.position - diff,
                20 * Time.deltaTime
                );
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
        if(collision.gameObject.tag.Contains("UnityChan"))
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
