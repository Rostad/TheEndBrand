using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState3D : ICharacterState3D
{

    private readonly Controller3D _Controller;
    private readonly Velocity3D _Velocity;
    private float _RunMultiplier = 1.35f;


    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public RunState3D(Controller3D controller, Velocity3D velocity)
    {
        _Controller = controller;
        _Velocity = velocity;
    }

    public CharacterStateSwitch3D HandleCollisions(CollisionFlags collisionFlags)
    {
        CharacterStateSwitch3D stateSwitch;
        if ((collisionFlags & CollisionFlags.Below) == CollisionFlags.Below)
        {
            stateSwitch = new CharacterStateSwitch3D();
        }
        else
        {
            stateSwitch = new CharacterStateSwitch3D(new AirState3D(_Controller, _Velocity));
            _Controller.ChangeCharacterState(stateSwitch);
        }
        return stateSwitch;
    }

    public void Update(Vector3 movementInput, float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Space) && _Controller.CanDodge)
        {
            var stateSwitch = new CharacterStateSwitch3D(new DodgeState3D(_Controller, _Velocity, movementInput));
            _Controller.ChangeCharacterState(stateSwitch);
        }
        if (!Input.GetKey(KeyCode.LeftShift) || movementInput.z < 0.9f) {

            var stateSwitch = new CharacterStateSwitch3D(new GroundState3D(_Controller, _Velocity, true), movementInput, deltaTime, true);
            _Controller.ChangeCharacterState(stateSwitch);
        }

        UpdateVelocity(movementInput, deltaTime);
    }

    private void UpdateVelocity(Vector3 movementInput, float deltaTime)
    {
        
        var smoothDampDataX = GetSmoothDampData(movementInput.x * 0.5f);
        var smoothDampDataZ = GetSmoothDampData(movementInput.z);
        _Velocity.SetY(-20.0f);
        _Velocity.SmoothDampUpdate(movementInput, smoothDampDataX, smoothDampDataZ, deltaTime);
    }
    private SmoothDampData GetSmoothDampData(float input)
    {
        var targetVelocity = (input * _Controller.MoveSpeed) * _RunMultiplier;
        var smoothTime = _Controller.GroundAccelerationTime;
        if (Mathf.Abs(input) < MathHelper.FloatEpsilon)
        {
            smoothTime *= _Controller.GroundDeaccelerationScale;
        }
        return new SmoothDampData(targetVelocity, smoothTime);
    }
}
