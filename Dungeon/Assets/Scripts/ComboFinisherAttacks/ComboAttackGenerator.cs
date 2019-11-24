using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "attacks/Combos")]
public class ComboAttackGenerator : ScriptableObject
{
    public List<string> attackClips = new List<string>();
    private List<string> clips;
    private Queue<string> usedClips = new Queue<string>();
    private float minDamage = 30;
    private float maxDamage = 45;

    public void OnEnable()
    {
        clips = new List<string>(attackClips);
    }

    //Create combo attack and finisher attack. Should rename this script to comboattacklist or combolist. Maybe rename getattack to generateattack
    public ComboAttack GetAttack()
    {
        var index = UnityEngine.Random.Range(0, clips.Count - 1);
        int damage = (int)UnityEngine.Random.Range(minDamage, maxDamage);

        var clip = clips[index];
        if (usedClips.Count > 0)
        {
            clips.Add(usedClips.Dequeue());
        }
        usedClips.Enqueue(clip);
        clips.RemoveAt(index);

        return new ComboAttack(clip, damage);
       
    }

    
}
