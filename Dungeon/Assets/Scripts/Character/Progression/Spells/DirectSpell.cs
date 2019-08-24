using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DirectSpell")]

public class DirectSpell : Spell
{

    public ITargeting targetingSystem;



    public override void Initialize(GameObject obj)
    {
        source = obj;
    }

    public override void TriggerAbility()
    {
        List<ITargetable> targets = targetingSystem.GetTargets(source);
        foreach(ITargetable t in targets)
        {
            effect.Apply(t);
        }
    }
}
