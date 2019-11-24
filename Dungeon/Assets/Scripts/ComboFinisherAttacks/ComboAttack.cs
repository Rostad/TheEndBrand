using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ComboAttack 
{
    public string clip;
    public int damage;

    public ComboAttack(string c, int d)
    {
        clip = c;
        damage = d;
    }
}
