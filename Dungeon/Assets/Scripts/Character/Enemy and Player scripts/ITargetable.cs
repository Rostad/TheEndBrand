using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{

    bool ApplyStatus(StatusEffect effect);

    Health GetHealth();

    Mana GetMana();

}
