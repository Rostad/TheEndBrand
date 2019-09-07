using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDriver : MonoBehaviour
{
    public Hitbox rightArm;
    public Hitbox leftArm;
    public Hitbox leftLeg;
    public Hitbox rightLeg;

    public void EnableHitbox(string name)
    {
        switch (name)
        {
            case "Right arm":
                rightArm.EnableHitBox();
                break;
            case "Left arm":
                leftArm.EnableHitBox();
                break;
            case "Left leg":
                leftLeg.EnableHitBox();
                break;
            case "Right leg":
                rightLeg.EnableHitBox();
                break;
            default:
                break;
        }
    }
}
