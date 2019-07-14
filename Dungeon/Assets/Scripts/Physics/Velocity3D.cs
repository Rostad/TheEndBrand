using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Velocity3D {

    private Vector3 velocity;
    private Vector3 velocityDampSmoothing;
    private float terminalVelocity;
    private float deltaTime;

    public Velocity3D(float terminalVelocity)
    {
        if (terminalVelocity > 0.0f)
            throw new ArgumentOutOfRangeException("terminalVelocity", terminalVelocity, "The terminal velocity must be negative.");

        this.terminalVelocity = terminalVelocity;
    }

    public Vector3 Current
    {
        get { return velocity * deltaTime;  }
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void ZeroVelocity()
    {
        velocity = Vector3.zero;
    }

    public void SmoothDampUpdate(Vector3 movementInput, SmoothDampData smoothDampDataX, SmoothDampData smoothDampDataZ, float deltaTime)
    {
        velocity.x = Mathf.SmoothDamp(velocity.x, smoothDampDataX.TargetVelocity, ref velocityDampSmoothing.x, smoothDampDataX.SmoothTime);
        velocity.z = Mathf.SmoothDamp(velocity.z, smoothDampDataZ.TargetVelocity, ref velocityDampSmoothing.z, smoothDampDataZ.SmoothTime);
        this.deltaTime = deltaTime;
    }

    public void AddY(float velocityY)
    {
        velocity.y += velocityY;
        ClampVelocityYtoTerminalVelocity();
    }

    public void SetY(float velocityY)
    {
        velocity.y = velocityY;
        ClampVelocityYtoTerminalVelocity();
    }

    private void ClampVelocityYtoTerminalVelocity()
    {
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);
    }
	
}
