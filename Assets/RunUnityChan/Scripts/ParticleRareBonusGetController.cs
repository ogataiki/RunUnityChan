using UnityEngine;
using System.Collections;

public class ParticleRareBonusGetController : MonoBehaviour {

    private ParticleSystem _particleSystem;

    // Use this for initialization
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ParticleStart()
    {
        _particleSystem.Play();
    }
    public void ParticleStop()
    {
        _particleSystem.Stop();
    }
    public void Finish()
    {
        Destroy(this.gameObject);
        _particleSystem.Stop();
    }
}
