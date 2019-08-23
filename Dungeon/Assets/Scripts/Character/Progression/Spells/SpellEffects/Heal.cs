using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Heal")]
public class Heal : Effect
{
    public override void Apply(ITargetable target, int power)
    {
        target.DoHeal(power);
    }
}
