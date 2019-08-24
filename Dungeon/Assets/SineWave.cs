using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour {

    float angle;
    float speed;
    Vector3 center;

	// Use this for initialization
	void Start () {
        angle = 0.0f;
        speed = 1f * Time.deltaTime;
        center = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(angle < Mathf.PI * 2)
        {
            var x = angle * 20;
            var y = Mathf.Cos(angle) * 20;
            transform.position = new Vector3(x, y);
            angle += speed;
        }
	}

    private void printSine()
    {
        for (var a = 0.0f; a < Mathf.PI * 2; a += .01f)
        {
            Debug.Log(Mathf.Sin(a));
        }
    }

    private void printSineDegrees()
    {
        for (var a = 0.0f; a < Mathf.PI * 2; a += .01f)
        {
            Debug.Log(getDegrees(a));
        }
    }

    public float getDegrees(float f)
    {
        return (f * 180) / Mathf.PI;
    }

    public float getRadians(float f)
    {
        return (f * Mathf.PI) / 180;
    }

   /* public void OnDrawGizmos()
    {
        for (var a = 0.0f; a < Mathf.PI * 2; a += .01f)
        {
            var x = a * 20;
            var y = Mathf.Cos(a)* 20;
            var v = new Vector3(x, y);
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(v, new Vector3(1, 1, 1));
        }
        for (var a = 0.0f; a < Mathf.PI * 2; a += .01f)
        {
            var x = a * 20;
            var y = Mathf.Sin(a) * 20;
            var v = new Vector3(x, y);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(v, new Vector3(1, 1, 1));
        }
        for (var a = 0.0f; a < Mathf.PI * 2; a += .01f)
        {
            var x = a * 20;
            var y = Mathf.Tan(a) * 20;
            var v = new Vector3(x, y);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(v, new Vector3(1, 1, 1));
        }
    }*/
}
