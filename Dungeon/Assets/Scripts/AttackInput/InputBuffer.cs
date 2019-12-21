using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer {

    private Queue<AttackInput> _ButtonBuffer;
    private KeyCode _KickButton, _PunchButton;

    public void Initialize()
    {
        _ButtonBuffer = new Queue<AttackInput>();
        _KickButton = KeyCode.JoystickButton7;
        _PunchButton = KeyCode.JoystickButton5;
    }

    public void TryAdd(Vector3 Dir)
    {
        InputDirection direction = InputDirection.NO_VALUE;

        if (Dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(Dir.z, Dir.x) * Mathf.Rad2Deg;
            direction = EvaluateDirection(angle);
        }
        else
        {
            direction = InputDirection.Neutral;
        }
        AttackButton button = GetButton();
        if(button == AttackButton.NONE)
        {
            return;
        }
        AttackInput input = new AttackInput(direction, button);
        _ButtonBuffer.Enqueue(input);
    }

    public AttackButton GetButton()
    {
        var v = CheckButton();
        return v;
    }

    public AttackButton CheckButton()
    {
       
        
        if (Input.GetKeyDown(_KickButton))
            return AttackButton.Kick;
        if (Input.GetKeyDown(_PunchButton))
            return AttackButton.Punch;
        return AttackButton.NONE;

        
    }

    public void Clear()
    {
        _ButtonBuffer = new Queue<AttackInput>();
    }

    

    public InputDirection EvaluateDirection(float angle)
    {
        if ((90 - 45) < angle && angle < (90 + 45))
            return InputDirection.Forward;
        if ((-90 - 45) < angle && angle < (-90 + 45))
            return InputDirection.Back;
        if ((180 - 45) < angle || angle < (-180 + 45))
            return InputDirection.Left;
        if ((0 - 45) < angle && angle < (0 + 45))
            return InputDirection.Right;


        return InputDirection.Neutral;
    }

    public bool IsEmpty()
    {
        return _ButtonBuffer.Count == 0;
    }

    public AttackInput Dequeue()
    {
        return _ButtonBuffer.Dequeue();
    }

    
}
