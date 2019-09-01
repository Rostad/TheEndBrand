using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DirectSpell : Spell
{



    public override void Initialize(GameObject obj)
    {
        source = obj;
    }

    public override void TriggerAbility()
    {
        foreach(Effect e in effects)
        {
            List<ITargetable> targets = e.targetingSystem.GetTargets(source);
            foreach(ITargetable t in targets)
            {
                e.Apply(t);
            }
        }
    }
}
