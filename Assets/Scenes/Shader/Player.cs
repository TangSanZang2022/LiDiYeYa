using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
public float speed = 200f;


    float xSpeed = 250.0f;
    float ySpeed = 120.0f;
    float yMinLimit = -20;
    float yMaxLimit = 80;
    private float x = 0.0f;
    private float y = 0.0f;  
    // Use this for initialization
    void Start () {
        var angles = transform.eulerAngles;  
        x = angles.y;  
        y = angles.x;  
    }
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }  
    // Update is called once per frame
    void Update () {

        //w键前进 
        if (Input.GetKey(KeyCode.W))
        {
            this.gameObject.transform.Translate(new Vector3(0, 0,10*speed* Time.deltaTime), this.gameObject.transform);
        }
        //s键后退 
        if (Input.GetKey(KeyCode.S))
        {
            this.gameObject.transform.Translate(new Vector3(0, 0, -1 * 5*speed * Time.deltaTime), this.gameObject.transform);
        }
        //a键后退 
        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(new Vector3(-1 * 10*speed * Time.deltaTime, 0, 0), this.gameObject.transform);
        }
        //d键后退 
        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(new Vector3(Time.deltaTime * 10*speed, 0, 0), this.gameObject.transform);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            this.gameObject.transform.Translate(new Vector3(0, Time.deltaTime * 5*speed, 0), this.gameObject.transform);
        }
        if (Input.GetKey(KeyCode.E))
        {
            this.gameObject.transform.Translate(new Vector3(0, -1 * Time.deltaTime * 5*speed, 0), this.gameObject.transform);
        }
        if (Input.GetMouseButton(0))
        {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                var rotation = Quaternion.Euler(y, x, 0);
                transform.rotation = rotation;

        } 
    }
}
