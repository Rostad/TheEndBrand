using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCameraController : MonoBehaviour
{

    public static FirstCameraController Instance;

    public Transform TargetLookAt;
    public float DefaultDistance = 5f;
    public float SmoothDistance = 0.05f;
    public float MouseSensitivityY = 3f;
    public float MouseSensitivityX = 3f;
    public float MinLimitY = -40f;
    public float MaxLimitY = 80f;
    public float SmoothY = 0.05f;
    public float SmoothZ = 0.05f;

    private float _Distance;
    private int _LayerMask = 1 << 9;
    private float _MouseX;
    private float _MouseY;
    private float _DesiredDistance;
    private float _VelDirection = 0;
    private float _VelX = 0;
    private float _VelY = 0;
    private float _VelZ = 0;
    private Vector3 _Position = Vector3.zero;
    private Vector3 _DesiredPosition = Vector3.zero;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
     

    }

    void LateUpdate()
    {

        if (TargetLookAt == null)
            return;

        HandleInput();

        CalculateDesiredPosition();

        UpdatePosition();

      
    }

    private void HandleInput()
    {
        _MouseY -= Input.GetAxis("Mouse Y") * MouseSensitivityY;
        _MouseX += Input.GetAxis("Mouse X") * MouseSensitivityX;
        _MouseY = CameraHelper.ClampAngle(_MouseY, MinLimitY, MaxLimitY);

        _DesiredDistance = CalculateDesiredDistance();
    }

    private float CalculateDesiredDistance()
    {
        var heading = this.transform.position - TargetLookAt.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        Debug.DrawRay(TargetLookAt.position, heading, Color.black);

        RaycastHit hit;
        if (Physics.Raycast(TargetLookAt.position, direction, out hit, DefaultDistance, _LayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.distance < DefaultDistance)
                return hit.distance * 0.9f;
           
        }

        return DefaultDistance;
    }

    private void CalculateDesiredPosition()
    {
        _Distance = Mathf.SmoothDamp(_Distance, _DesiredDistance, ref _VelDirection, SmoothDistance);
        

        _DesiredPosition = CalculatePosition(_MouseY, _MouseX, _Distance);

    }

    private Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {

        Vector3 direction = new Vector3(0, 0, -_Distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        return TargetLookAt.position + (rotation * direction);
    }

    private void UpdatePosition()
    {
        var posX = Mathf.SmoothDamp(_Position.x, _DesiredPosition.x, ref _VelX, Time.deltaTime);
        var posY = Mathf.SmoothDamp(_Position.y, _DesiredPosition.y, ref _VelY, Time.deltaTime);
        var posZ = Mathf.SmoothDamp(_Position.z, _DesiredPosition.z, ref _VelZ, Time.deltaTime);
        _Position = new Vector3(posX, posY, posZ);

        transform.position = _Position;
        transform.LookAt(TargetLookAt);
    }

    

    public static void UseExistingOrCreateMainCamera(Controller3D controller)
    {
        GameObject tempCamera;
        GameObject targetLookAt;
        FirstCameraController myCameraController;
        


        if (Camera.main != null)
        {
            tempCamera = Camera.main.gameObject;
        }
        else
        {
            tempCamera = new GameObject("Main Camera");
            tempCamera.AddComponent(typeof(Camera));
            tempCamera.tag = "MainCamera";
        }

        tempCamera.AddComponent(typeof(FirstCameraController));
        myCameraController = tempCamera.GetComponent<FirstCameraController>();

        targetLookAt = GameObject.Find("targetLookAt");

        if (targetLookAt == null)
        {
            targetLookAt = new GameObject("targetLookAt");
            targetLookAt.transform.position = Vector3.zero;
        }

        myCameraController.TargetLookAt = targetLookAt.transform;
        
    }
}
