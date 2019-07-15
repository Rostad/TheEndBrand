using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SlideState3D : ICharacterState3D
{
    private readonly Controller3D controller;

    private readonly Velocity3D velocity;

    public SlideState3D(Controller3D controller, Velocity3D velocity)
    {
        if (controller == null)
            throw new ArgumentNullException("controller");
        if (velocity == null)
            throw new ArgumentNullException("velocity");

        this.controller = controller;
        this.velocity = velocity;
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
        if((collisionFlags & CollisionFlags.Below) == CollisionFlags.Below){
            stateSwitch = new CharacterStateSwitch3D();
            RaycastHit hitInfo;
            if(Physics.Raycast(controller.transform.position, Vector3.down, out hitInfo, controller.ColliderHeight))
            {
                if(Vector3.Angle(hitInfo.normal, Vector3.up) <= controller.MaxTraversableSlopeAngle)
                {
                    stateSwitch = new CharacterStateSwitch3D(new GroundState3D(controller, velocity, true));
                }
            }
            else
            {
                stateSwitch = new CharacterStateSwitch3D(new AirState3D(controller, velocity));
            }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(controller.transform.position, Vector3.down, out hitInfo, controller.ColliderHeight))
            {
                movementInput = hitInfo.normal * controller.SlideJumpSpeed;
            }
            var stateSwitch = new CharacterStateSwitch3D(new AirState3D(controller, velocity), movementInput, deltaTime, true);
            controller.ChangeCharacterState(stateSwitch);
        }
        else
        {
            UpdateVelocity(movementInput, deltaTime);
        }
    }

    private void UpdateVelocity(Vector3 movementInput, float deltaTime)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(controller.transform.position, Vector3.down, out hitInfo, controller.ColliderHeight))
        {
            var normalVectorWithInvertedY = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
            movementInput = controller.transform.InverseTransformDirection(normalVectorWithInvertedY).normalized;
        }
        var smoothDampDataX = GetSmoothDampData(movementInput.x);
        var smoothDampDataZ = GetSmoothDampData(movementInput.z);

        velocity.AddY(controller.Gravity * deltaTime);
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
