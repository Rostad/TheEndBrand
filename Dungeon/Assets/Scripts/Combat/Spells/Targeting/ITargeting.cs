using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITargeting : ScriptableObject
{

    public abstract List<ITargetable> GetTargets(GameObject source);

}
