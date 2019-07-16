using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    HitData DoDamage(AttackData attack);
}
