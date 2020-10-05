using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float playerSpeed = 5f;
    public float playerJumpForce = 1f;
    public float maxJumpTime = 1;
    public float fallMultiplayer = 2;

    public float lookSpeed = 5f;
    
    public Transform fpsCamera;
    Rigidbody rb;

    float timeSincePressed;
    bool isGrounded;
    bool ignoreJumpInput;

    float time;


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

        
        if (isGrounded)
        {
			timeSincePressed = 0;
            time = 0;
            ignoreJumpInput = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && time == 0 && isGrounded)
		{
			time = Time.time;
            //Debug.Log(timeSincePressed);
		}

        
        if (Input.GetKey(KeyCode.Space) && time != 0)
		{
            
            timeSincePressed = Time.time - time + 0.01f;
            Jump(timeSincePressed);
		}

        
        if (timeSincePressed > maxJumpTime)
		{
            Debug.Log("test");
            ignoreJumpInput = true;
            SendDown();
		}


        if (!ignoreJumpInput && Input.GetAxis("Jump") == 0 && !isGrounded)
		{
            SendDown();
		}
    }

    void SendDown()
	{
        rb.AddForce(Vector3.down * fallMultiplayer * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnCollisionStay(Collision collision)
	{
		for (int i = 0; i < collision.contactCount; i++)
		{
            // we check that the contact point is below the player
            float dot = Vector3.Dot(-transform.up, collision.GetContact(i).point - (-transform.up)); // We use -transform.up instead of adding for the sake of clarity

            if (dot < 0)
			{
                isGrounded = true;
                break;
			}
			else
			{
                isGrounded = false;
			}

		}
	}

	private void OnCollisionExit(Collision collision)
	{
        isGrounded = false;
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

    void Jump(float timePassed)
	{
        //rb.mass = maxJumpTime - timePassed;
        rb.AddForce(Vector3.up * playerJumpForce * (maxJumpTime - timePassed), ForceMode.Impulse);

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