using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{

    public abstract void Apply(ITargetable target, int power);

}
