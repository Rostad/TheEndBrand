using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DodgeState3D : ICharacterState3D
{
    private Controller3D _Controller;
    private Velocity3D _Velocity;
    private Vector3 _Direction;
    private float _DodgeMultiplier;
    private float _DodgeDuration;
    private float _Timer;
    private bool _Enter;

    public void Enter()
    {

    }

    public void Exit()
    {
        GameObject.FindGameObjectWithTag("Character").GetComponent<DummyAnimationController>().ResetRotation();
        _Controller.timeOfLastDodge = Time.time;
    }

    public DodgeState3D(Controller3D controller, Velocity3D velocity, Vector3 Direction)
    {
        if (controller == null)
            throw new ArgumentNullException("controller");

        if (velocity == null)
            throw new ArgumentNullException("velocity");
        _DodgeMultiplier = 2.0f;
        _Controller = controller;
        _Velocity = velocity;
        //_Direction = Direction.normalized;
        _Direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        _Direction.Normalize();
        _DodgeDuration = 0.5f;
        _Timer = 0;
        _Enter = true;

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
       /*if (Input.GetKeyDown(KeyCode.Space))
        {
            var stateSwitch = new CharacterStateSwitch3D(new AirState3D(_Controller, _Velocity, 3f), movementInput, deltaTime, true);
            _Controller.ChangeCharacterState(stateSwitch);
        }*/
        if (_Timer >= _DodgeDuration)
        {
            var stateSwitch = new CharacterStateSwitch3D(new GroundState3D(_Controller, _Velocity, false), movementInput, deltaTime, true);
            _Controller.ChangeCharacterState(stateSwitch);
        }
        else
        {
            UpdateVelocity(deltaTime);
            _Timer += deltaTime;
            if (_Enter)
            {
                GameObject.FindGameObjectWithTag("Character").GetComponent<DummyAnimationController>().PlayRoll();
                _Enter = false;
            }
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
        var targetVelocity = (direction * _Controller.MoveSpeed) * _DodgeMultiplier;
        var smoothTime = 0.01f;
        return new SmoothDampData(targetVelocity, smoothTime);
    }
}

