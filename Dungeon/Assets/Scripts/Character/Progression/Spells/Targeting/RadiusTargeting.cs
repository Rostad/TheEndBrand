using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusTargeting : ITargeting
{
    public float radius;
    private LayerMask mask = LayerMask.GetMask("Characters");

    public List<ITargetable> GetTargets(GameObject source)
    {
        List<ITargetable> targets = new List<ITargetable>();
        var hits = Physics.SphereCastAll(source.transform.position, radius, Vector3.zero, mask);
        foreach(RaycastHit h in hits)
        {
            ITargetable target = h.transform.gameObject.GetComponent<ITargetable>();
            if (target == null)
                throw new MissingComponentException(h.transform.name +  " is in character layer without a component implementing ITargetable");

            targets.Add(target);
        }

        return targets;
    }
}
