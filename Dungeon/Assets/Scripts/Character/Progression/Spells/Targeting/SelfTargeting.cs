using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTargeting : ITargeting
{
    public List<ITargetable> GetTargets(GameObject source)
    {
        return new List<ITargetable>(new ITargetable[] { source.GetComponent<ITargetable>() });
    }

    
}
