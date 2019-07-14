using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInputComparer : IEqualityComparer<AttackInput>
{
    public bool Equals(AttackInput x, AttackInput y)
    {
        if (x.Motion == y.Motion && x.Button == y.Button)
            return true;

        return false;
    }

    public int GetHashCode(AttackInput obj)
    {
        int value = 17;
        value = 37 * value + (int)obj.Motion;
        value = 37 * value + (int)obj.Button;
        return value;
    }
}
