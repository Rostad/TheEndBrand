using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargeting
{

    List<ITargetable> GetTargets(GameObject source);

}
