using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float playerSpeed = 5f;
    public float playerJumpForce = 1f;
    public float lookSpeed = 5f;

    public Transform fpsCamera;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Look();

    }



    void Move()
    {
        //---MOVE---//

        float axisV = Input.GetAxisRaw("Vertical");     // Forward/Backward Input
        float axisH = Input.GetAxisRaw("Horizontal");   // Left/Right Input

        Vector3 force = new Vector3(axisH, 0, axisV).normalized * playerSpeed * Time.deltaTime;

        //transform.Translate(new Vector3(axisH, 0, axisV).normalized * playerSpeed * Time.deltaTime);
        rb.AddForce(transform.TransformDirection(force), ForceMode.Impulse);
    }

    void Look()
    {
        //---LOOK---//

        Vector3 look = fpsCamera.localEulerAngles;

        // Gets mouse position
        look.x -= Input.GetAxis("Mouse Y") * lookSpeed;
        look.y += Input.GetAxis("Mouse X") * lookSpeed;

        //Debug.Log(look);

        // Turns player in y axis.
        fpsCamera.parent.localEulerAngles += new Vector3(0, look.y);

        // Stops player from looking to far up/down.
        if (look.x > 85 && look.x < 180)
        {
            fpsCamera.localEulerAngles = new Vector3(85, 0);
        }
        else if (look.x < 275 && look.x > 180)
        {
            fpsCamera.localEulerAngles = new Vector3(275, 0);
        }
        else
        {
            fpsCamera.localEulerAngles = new Vector3(look.x, 0);
        }
    }

}