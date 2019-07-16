using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public HitData DoDamage(AttackData attack)
    {
        return new HitData(this.name ,attack.damage, attack.attackEffect);
    }
}
