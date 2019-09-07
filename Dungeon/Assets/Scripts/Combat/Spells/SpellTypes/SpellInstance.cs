using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/SpellInstance")]
public class SpellInstance : Spell
{



    public override void Initialize(GameObject obj)
    {
        source = obj;
    }

    public override void TriggerAbility()
    {
        foreach(Effect e in effects)
        {
            e.Perform(source);
        }
    }
}
