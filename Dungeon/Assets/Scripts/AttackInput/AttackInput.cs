using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackInput {

    public InputDirection Motion;
    public AttackButton Button;

    public AttackInput(InputDirection motion, AttackButton button)
    {
        Motion = motion;
        Button = button;
    }

   
	
}
