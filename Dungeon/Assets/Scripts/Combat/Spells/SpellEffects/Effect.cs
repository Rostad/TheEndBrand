using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public ITargeting targetingSystem;

    public abstract void Apply(ITargetable target);

}
