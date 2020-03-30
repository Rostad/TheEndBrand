using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDriver : MonoBehaviour
{
    public Hitbox sword;

    private Player player;

    public void EnableHitbox()
    {
        sword.EnableHitBox();
    }

    public void DoHit(string origins)
    {
        string[] originsSplit = origins.Split( ',');
        player.DoHit(originsSplit);
    }

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
}
