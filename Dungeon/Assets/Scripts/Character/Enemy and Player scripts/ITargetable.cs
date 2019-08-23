using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{

    bool ApplyStatus(StatusEffect effect);

    float DoDamage(int amount);

    float DoHeal(int amount);

    float DoManaDamage(int amount);

    float DoManaRestore(int amount);

}
