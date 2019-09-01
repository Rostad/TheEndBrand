using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Targeting/Self")]
public class SelfTargeting : ITargeting
{
    public override List<ITargetable> GetTargets(GameObject source)
    {
        return new List<ITargetable>(new ITargetable[] { source.GetComponent<ITargetable>() });
    }

    
}
