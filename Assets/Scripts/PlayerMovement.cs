using System;
using UnityEngine;
using UnityEngine.Splines;


public class PlayerMovement : MonoBehaviour
{
    public float maxPlayerSpeed = 3f;
    float speed = 0f;
    MoveAlongSpline moveAlongSpline;

    void Start()
    {
        moveAlongSpline = GetComponent<MoveAlongSpline>();
        moveAlongSpline.spline = FindObjectOfType<SplineContainer>();
    }

    void Update()
    {
        ManageSpeed();
        ManageSpline();
    }

    private void ManageSpline()
    {
        moveAlongSpline.speed = speed;
    }

    void ManageSpeed()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            speed += 5f * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speed -= 5f * Time.deltaTime;
        }
        else
        {
            // Gradually return to zero speed when no input is given
            if (speed > 0)
            {
                speed -= 5f * Time.deltaTime;
                if (speed < 0) speed = 0;
            }
            else if (speed < 0)
            {
                speed += 5f * Time.deltaTime;
                if (speed > 0) speed = 0;
            }
        }

        speed = Mathf.Clamp(speed, -maxPlayerSpeed, maxPlayerSpeed);
    }
}
