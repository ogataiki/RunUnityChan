using UnityEngine;
using System.Collections;

public class ParticleBonusGetController : MonoBehaviour {

    private ParticleSystem _particleSystem;

    [SerializeField]
    private float particleTimeLimit = 2.0f;
    private float elapsedTime = 0.0f;

    // Use this for initialization
    void Start () {
        _particleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if(particleTimeLimit <= elapsedTime)
        {
            Destroy(this.gameObject);
            _particleSystem.Stop();
        }
    }
}
