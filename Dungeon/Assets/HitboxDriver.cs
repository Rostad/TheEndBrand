using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDriver : MonoBehaviour
{

    public Hitbox sword;
    public Hitbox leftLeg;
    public Hitbox rightLeg;

    public void EnableHitbox(string name)
    {
        switch (name)
        {
            case "Sword":
                sword.EnableHitBox();
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
