using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public struct GroundState3D : ICharacterState3D {

    private readonly Controller3D controller;

    private readonly Velocity3D velocity;

    private float timeEnter;

    private float dodgeCoolDown;
    
    public GroundState3D(Controller3D controller, Velocity3D velocity, bool canDodgeOnEnter)
    {
        if (controller == null)
            throw new ArgumentNullException("controller");

        if (velocity == null)
            throw new ArgumentNullException("velocity");

        this.controller = controller;
        this.velocity = velocity;
        if (!canDodgeOnEnter)
        {
            timeEnter = Time.time;
            dodgeCoolDown = 0.6f;
        }
        else
        {
            timeEnter = 0;
            dodgeCoolDown = 0f;
        }
        
    }

    public void Enter()
    {
        velocity.SetY(0.0f);
    }

    public void Exit()
    {

    }

    public CharacterStateSwitch3D HandleCollisions(CollisionFlags collisionFlags)
    {
        CharacterStateSwitch3D stateSwitch;
        if((collisionFlags & CollisionFlags.Below) == CollisionFlags.Below)
        {
            stateSwitch = new CharacterStateSwitch3D();
        }
        else
        {
            stateSwitch = new CharacterStateSwitch3D(new AirState3D(controller, velocity));
            controller.ChangeCharacterState(stateSwitch);
        }
        return stateSwitch;
    }

    public void Update(Vector3 movementInput, float deltaTime)
    {
        Vector3 v = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        /*if (Input.GetKeyDown(KeyCode.JoystickButton2) && controller.CanDodge && (v.z != 0 || v.x != 0))
        {
            var stateSwitch = new CharacterStateSwitch3D(new DodgeState3D(controller, velocity, movementInput));
            controller.ChangeCharacterState(stateSwitch);
        }
        else*/ if (Input.GetKeyDown(KeyCode.JoystickButton10) && movementInput.z > 0.9)
        {
            var stateSwitch = new CharacterStateSwitch3D(new RunState3D(controller, velocity));
            controller.ChangeCharacterState(stateSwitch);
        }
        /*else if(Input.GetKeyDown(KeyCode.Space))
        {
            var stateSwitch = new CharacterStateSwitch3D(new AirState3D(controller, velocity), movementInput, deltaTime, true);
            controller.ChangeCharacterState(stateSwitch);
        }*/
        else
        {
            UpdateVelocity(movementInput, deltaTime);
        }
        
        
    }

    private void UpdateVelocity(Vector3 movementInput, float deltaTime)
    {
        var smoothDampDataX = GetSmoothDampData(movementInput.x * 0.8f);
        var smoothDampDataZ = GetSmoothDampData(movementInput.z);
        velocity.SetY(-20.0f);
        velocity.SmoothDampUpdate(movementInput, smoothDampDataX, smoothDampDataZ, deltaTime);
    }
    private SmoothDampData GetSmoothDampData(float input)
    {
        var targetVelocity = input * controller.MoveSpeed;
        var smoothTime = controller.GroundAccelerationTime;
        if(Mathf.Abs(input) < MathHelper.FloatEpsilon)
        {
            smoothTime *= controller.GroundDeaccelerationScale;
        }
        return new SmoothDampData(targetVelocity, smoothTime);
    }
}
