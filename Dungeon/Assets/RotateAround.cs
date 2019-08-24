using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    public GameObject obj;
    float centerX;
    float centerY;
    float centerZ;
    float radius;
    public float angle;
    float speed;
    float slice;
	// Use this for initialization
	void Start () {
        speed = 3f * Time.deltaTime;
        radius = 2;
        slice = (Mathf.PI * 2) / 8;
    }
	
	// Update is called once per frame
	void Update () {
        centerX = obj.transform.position.x;
        centerY = obj.transform.position.y;
        centerZ = obj.transform.position.z;
        var i = angle * slice;
        var z = centerZ + Mathf.Sin(i) * radius;
        var x = centerX + Mathf.Cos(i) * radius;
        var y = centerY + Mathf.Sin(i) * radius;
        transform.position = new Vector3(x, centerY, z);
        angle += speed;
        
	}
    public float GetDegrees(float f)
    {
        return (f * 180) / Mathf.PI;
    }

    public float GetRadians(float f)
    {
        return (f * Mathf.PI) / 180;
    }
}
