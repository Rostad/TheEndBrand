using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    private List<ParticleController> particleControllers;

    // Start is called before the first frame update
    void Start()
    {
        particleControllers = new List<ParticleController>(GetComponentsInChildren<ParticleController>());
    }


    public void PlayParticles(string name)
    {
        foreach(ParticleController p in particleControllers)
        {
            if (p.name == name)
                p.Play();
        }
    }

    public void StopParticles(string name)
    {
        foreach (ParticleController p in particleControllers)
        {
            if (p.name == name)
                p.Stop();
        }
    }

}
