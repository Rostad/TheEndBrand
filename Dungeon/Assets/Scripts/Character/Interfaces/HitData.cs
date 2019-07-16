using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitData
{
    public string targetHit;
    public int damage;
    public AttackEffect effect;

    public HitData(string target, int damage, AttackEffect effect)
    {
        targetHit = target;
        this.damage = damage;
        this.effect = effect;
    }
}
