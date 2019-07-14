using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DashState3D : ICharacterState3D {

    private Vector3 _Direction;
    private readonly Controller3D _Controller;
    private readonly Velocity3D _Velocity;
    private readonly float _DashMultiplier;
    private float _DashDuration;
    private float _Timer;

    public DashState3D(Controller3D controller, Velocity3D velocity, Vector3 direction)
    {
        if (controller == null)
            throw new ArgumentNullException("controller");

        if (velocity == null)
            throw new ArgumentNullException("velocity");

        _Controller = controller;
        _Velocity = velocity;
        _Direction = direction;
        _Timer = 0;
        _DashDuration = 0.4f;
        _DashMultiplier = 5f;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
     
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
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            var stateSwitch = new CharacterStateSwitch3D(new DodgeState3D(_Controller, _Velocity, movementInput));
            _Controller.ChangeCharacterState(stateSwitch);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            var stateSwitch = new CharacterStateSwitch3D(new AirState3D(_Controller, _Velocity), movementInput, deltaTime, true);
            _Controller.ChangeCharacterState(stateSwitch);
        }
        else if(_Timer >= _DashDuration)
        {
            var stateSwitch = new CharacterStateSwitch3D(new GroundState3D(_Controller, _Velocity, true), movementInput, deltaTime, true);
            _Controller.ChangeCharacterState(stateSwitch);
        }
        else
        {
            UpdateVelocity(deltaTime);
            _Timer += deltaTime;
        }
    }

    private void UpdateVelocity(float deltatime)
    {
        var smoothDampDataX = GetSmoothData(_Direction.x);
        var smoothDampDataZ = GetSmoothData(_Direction.z);
        _Velocity.SetY(-20f);
        _Velocity.SmoothDampUpdate(_Direction, smoothDampDataX, smoothDampDataZ, deltatime);
    }

    private SmoothDampData GetSmoothData(float direction)
    {
        var targetVelocity = (direction * _Controller.MoveSpeed) * _DashMultiplier;
        var smoothTime = _Controller.GroundAccelerationTime;
        return new SmoothDampData(targetVelocity, smoothTime);
    }

   
}
