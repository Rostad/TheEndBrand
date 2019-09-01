using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{

    private ITargetable affected;

    public abstract void Update();
    public abstract void OnApply();
    public abstract void OnRemove();
}
