using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Effects/Damage")]
public class Damage : Effect
{
    public override void Apply(ITargetable target)
    {
        //target.DoDamage();
    }
}
