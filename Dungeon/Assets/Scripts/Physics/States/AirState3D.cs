using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AirState3D : ICharacterState3D
{
    private const int maxJumpCount = 0;

    private readonly Controller3D controller;

    private readonly Velocity3D velocity;

    private int jumpCount;

    private float _VelocityMultiplier;

    private float _MinMultiplier;

    public AirState3D(Controller3D controller, Velocity3D velocity)
    {
        if (controller == null)
            throw new ArgumentNullException("controller");

        if (velocity == null)
            throw new ArgumentNullException("velocity");

        this.controller = controller;
        this.velocity = velocity;
        _VelocityMultiplier = 1.8f;
        _MinMultiplier = 0.8f;
        jumpCount = 0;
    }
    public AirState3D(Controller3D controller, Velocity3D velocity, float v)
    {
        if (controller == null)
            throw new ArgumentNullException("controller");

        if (velocity == null)
            throw new ArgumentNullException("velocity");

        this.controller = controller;
        this.velocity = velocity;
        _VelocityMultiplier = v;
        _MinMultiplier = 0.8f;
        jumpCount = 0;
    }
    public void Enter()
    {
        velocity.SetY(0.0f);
    }

    public void Exit(){}

    public CharacterStateSwitch3D HandleCollisions(CollisionFlags collisionFlags)
    {
        CharacterStateSwitch3D stateSwitch;
        if((collisionFlags & CollisionFlags.Below) == CollisionFlags.Below)
        {
            if(controller.IsTraversableSlope(controller.ColliderHeight * 10.0f))
            {
                stateSwitch = new CharacterStateSwitch3D(new GroundState3D(controller, velocity, true));
            }
            else
            {
                stateSwitch = new CharacterStateSwitch3D(new SlideState3D(controller, velocity));
            }
        }
        else
        {
            stateSwitch = new CharacterStateSwitch3D();
        }
        return stateSwitch;
    }

    public void Update(Vector3 movementInput, float deltaTime)
    {
        if (ShouldJump())
            PerformJump();
        else if (ShouldMinJump())
            PerformMinJump();

        if (_VelocityMultiplier > _MinMultiplier)
        {
            _VelocityMultiplier -= 0.02f;
        }

        UpdateVelocity(movementInput, deltaTime);
    }

    private void PerformJump()
    {
        jumpCount++;
        velocity.SetY(controller.MaxJumpVelocity);
    }

    private void PerformMinJump()
    {
        velocity.SetY(controller.MinJumpVelocity);
    }

    private bool ShouldJump()
    {
        return Input.GetKeyDown(controller.JumpKey) && jumpCount < maxJumpCount;
    }

    private bool ShouldMinJump()
    {
        return Input.GetKeyDown(controller.JumpKey) && velocity.Current.y > controller.MinJumpVelocity;
    }

    private void UpdateVelocity(Vector3 movementInput, float deltaTime)
    {
        var smoothDampDataX = GetSmoothDampData(movementInput.x);
        var smoothDampDataZ = GetSmoothDampData(movementInput.z);

        velocity.AddY(controller.Gravity * deltaTime);
        velocity.SmoothDampUpdate(movementInput, smoothDampDataX, smoothDampDataZ, deltaTime);
    }

    private SmoothDampData GetSmoothDampData(float input)
    {
        var targetVelocity = (input * controller.MoveSpeed) * _VelocityMultiplier;
        var smoothTime = controller.AirAccelerationTime;
        if(Mathf.Abs(input) < MathHelper.FloatEpsilon)
        {
            smoothTime *= controller.AirDeaccelerationScale;
        }
        return new SmoothDampData(targetVelocity, smoothTime);
    }
}
