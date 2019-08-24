using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDriver : MonoBehaviour
{

    public ParticleManager[] particleManagers;
    public static ParticleDriver instance;

    public void PlayParticles(string name)
    {
        for(int i = 0; i<particleManagers.Length; i++)
        {
            particleManagers[i].PlayParticles(name);
        }
    }

    public void Start()
    {
        instance = this;
    }

    public void StopParticles(string name)
    {
        for (int i = 0; i < particleManagers.Length; i++)
        {
            particleManagers[i].StopParticles(name);
        }
    }
}
