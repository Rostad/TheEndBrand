using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    private List<ParticleSystem> particleSystems;

    // Start is called before the first frame update
    void Start()
    {
        particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
        foreach (ParticleSystem p in particleSystems)
        {
            p.time = 0;
            p.Stop();
        }
            
    }

    public void Play()
    {
        foreach (ParticleSystem p in particleSystems)
            p.Play();
    }

    public void Stop()
    {
        foreach (ParticleSystem p in particleSystems)
            p.Stop();
    }

}
