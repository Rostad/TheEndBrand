using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Effects/Damage")]
public class Damage : Effect
{

    public ITargeting targetingSystem;

    public override void Apply(ITargetable target)
    {
        //target.DoDamage();
    }

    public override void Perform(GameObject source)
    {
        var targets = targetingSystem.GetTargets(source);
        foreach(ITargetable t in targets)
        {
            Apply(t);
        }
    }
}
