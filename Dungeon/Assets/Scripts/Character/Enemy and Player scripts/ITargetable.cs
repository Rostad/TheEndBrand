using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{

    bool ApplyStatus(StatusEffect effect);

    void DoDamage(int amount);

    void DoHeal(int amount);

    void DoManaDamage(int amount);

    void DoManaRestore(int amount);

}
