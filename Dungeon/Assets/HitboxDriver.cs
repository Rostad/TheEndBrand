using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDriver : MonoBehaviour
{
    public Hitbox sword;

    public void EnableHitbox()
    {
        sword.EnableHitBox();
    }
}
