using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{

    public int damage;
    public AttackEffect attackEffect;
    public AttackEffect counterEffect;


    public AttackData(int damage, AttackEffect effect, AttackEffect counterEffect)
    {
        this.damage = damage;
        attackEffect = effect;
        this.counterEffect = counterEffect;
    }

}
