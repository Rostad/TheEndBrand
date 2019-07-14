using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controller3D : MonoBehaviour {

    #region Constants
    public readonly KeyCode JumpKey = KeyCode.Space;
    #endregion

    [Tooltip("Units per second")]
    public float MoveSpeed = 6.0f;

    [Tooltip("Degrees per second")]
    public float RotationSpeed = 120.0f;

    [Tooltip("Units per second when jumping while sliding down a slope")]
    public float SlideJumpSpeed = 10.0f;

    [Tooltip("Maximum downwards velocity(entered as a positive value")]
    public float TerminalVelocity = 18.0f;

    public float GroundAccelerationTime = 0.1f;

    public float AirAccelerationTime = 0.2f;

    public float GroundDeaccelerationScale = 0.8f;

    public float AirDeaccelerationScale = 3.5f;

    [Tooltip("The maximum jump height in units")]
    public float MaxJumpHeight = 4.0f;

    [Tooltip("The smallest jump height possible in units")]
    public float MinJumpHeight = 1.0f;

    [Tooltip("The time it takes(seconds) to reach the maximum jump height")]
    public float TimeToJumpApex = 0.4f;

    private CharacterController characterController;

    private Velocity3D velocity;

    private ICharacterState3D characterState;

    public bool canMove;

    public float MaxJumpVelocity { get; private set; }

    public float MinJumpVelocity { get; private set; }

    public float Gravity { get; private set; }

    public float timeOfLastDodge = 0f;

    public float dodgeCooldown = 0.6f;

    public float MaxTraversableSlopeAngle
    {
        get { return characterController.slopeLimit; }
    }

    public float ColliderHeight
    {
        get { return characterController.height; }
    }

    public bool CanDodge
    {
        get { return Time.time > timeOfLastDodge + dodgeCooldown; }
    }

    public bool CanAttack
    {
        get { return characterState.ToString() == "GroundState3D" || characterState.ToString() == "RunState3D"; }
    }

    public void ChangeCharacterState(CharacterStateSwitch3D stateSwitch)
    {
        PrintStateSwitch(stateSwitch);
        characterState.Exit();
        characterState = stateSwitch.NewState;
        characterState.Enter();
        if (stateSwitch.RunImmediately)
            characterState.Update(stateSwitch.MovementInput, stateSwitch.DeltaTime);
    }

    public bool IsTraversableSlope(float maxDistance)
    {
        var GroundSlopeAngle = 0.0f;
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, maxDistance))
            GroundSlopeAngle = Vector3.Angle(Vector3.up, hitInfo.normal);

        return GroundSlopeAngle <= MaxTraversableSlopeAngle;
    }

    private void Awake()
    {
        CreateVelocity();
        CacheComponents();
        CalculateGravity();
        CalculateJumpVelocities();
        SetInitialCharacterState();
        canMove = true;
    }

	void Update () {
        var deltaTime = Time.deltaTime;
        var v = GetMovementInput();
        RotateCharacter(deltaTime);
        characterState.Update(v, deltaTime);
        HandleCollisions(Move());
        DrawAxes();
        if (Input.GetKeyDown(KeyCode.G))
            Debug.Log(canMove);
	}

    

    private void CacheComponents()
    {
        characterController = GetComponent<CharacterController>();
        
    }

    private void CalculateGravity()
    {
        Gravity = -(2 * MaxJumpHeight / Mathf.Pow(TimeToJumpApex, 2));
    }

    private void CalculateJumpVelocities()
    {
        var positiveGravity = Mathf.Abs(Gravity);
        MaxJumpVelocity = positiveGravity * TimeToJumpApex;
        MinJumpVelocity = Mathf.Sqrt(2 * positiveGravity * MinJumpHeight);
    }

    private void CreateVelocity()
    {
        velocity = new Velocity3D(-TerminalVelocity);
        GetComponentInChildren<DummyAnimationController>().ShareVelocity(velocity);
    }

    private void SetInitialCharacterState()
    {
        if (characterController.isGrounded)
        {
            if(IsTraversableSlope(ColliderHeight * 10.0f))
            {
                characterState = new SlideState3D(this, velocity);
            }
            else
            {
                characterState = new GroundState3D(this, velocity, true);
            }
        }
        else
        {
            characterState = new AirState3D(this, velocity);
        }
    }

    public void RotateCharacter(float deltaTime)
    {
        var x = Input.GetAxisRaw("Mouse X");
        if(Mathf.Abs(x) > Mathf.Epsilon)
           transform.RotateAround(transform.position, transform.up, (x * RotationSpeed) * deltaTime);


    }
    
    private Vector3 GetMovementInput()
    {
        if (!canMove)
            return Vector3.zero;

        var v = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        return v;
    }

    private CollisionFlags Move()
    {
        var moveDirection = transform.TransformDirection(velocity.Current).normalized;
        var moveLength = velocity.Current.magnitude;
        var motion = moveDirection * moveLength;
        return characterController.Move(motion);
    }

    private void HandleCollisions(CollisionFlags collisionFlags)
    {
        var stateSwitch = characterState.HandleCollisions(collisionFlags);
        if (stateSwitch.NewState != null)
            ChangeCharacterState(stateSwitch);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void DrawAxes()
    {
        Debug.DrawRay(transform.position + transform.forward * characterController.radius, transform.forward, Color.blue);
        Debug.DrawRay(transform.position + transform.right * characterController.radius, transform.right, Color.red);
        Debug.DrawRay(transform.position + transform.up * characterController.height * 0.5f, transform.up, Color.green);
    }
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void PrintStateSwitch(CharacterStateSwitch3D stateSwitch)
    {
        print("Switching character state from " + characterState.ToString() + " to " + stateSwitch.NewState.ToString());
    }
}
