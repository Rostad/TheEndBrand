using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour, ISpawnableSpell
{

    private RotateAround[] spheres;
    private float duration;
    private float timer;

    public void SetUp(GameObject caster, Effect effect)
    {
        timer = 0;
        duration = 5f;
        spheres = GetComponentsInChildren<RotateAround>();
        int angle = 0;
        int angleIncrement = 360 / spheres.Length;
        for(int i = 0; i < spheres.Length; i++)
        {
            spheres[i].obj = caster;
            spheres[i].angle = angle;
            angle += angleIncrement;
        }
    }

    public void Update()
    {
        if(timer > duration)
        {
            for(int i = 0; i < spheres.Length; i++)
            {
                Destroy(spheres[i].gameObject);
            }
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
    }
}
