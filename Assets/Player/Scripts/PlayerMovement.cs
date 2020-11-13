using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float playerSpeed = 5f;
    public float playerJumpForce = 1f;
    public float fallForce = 10;
    public float jumpTime = 0.5f;

    [Range(0,1)]
    public float groundBias = 0.9f;
    [Range(0, 1)]
    public float wallBias = 0;
    public CapsuleCollider wallCollider;

    bool isGrounded = false;
    bool hasDoubleJumped = false;
    bool isOnWall = false;
    bool isMove = true;
    Vector3 wallNormal;

    public float lookSpeed = 5f;
    
    public Transform fpsCamera;
    Rigidbody rb;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    float endTime;
    // Update is called once per frame
    void Update()
    {
        
        if (!Cursor.visible)
        {
            if (isMove)
			{
                Move();
			}

            Look();

            if (isGrounded)
			{
                hasDoubleJumped = false;
                if (Input.GetButtonDown("Jump"))
				{
                    Jump();
                    endTime = Time.time + jumpTime;
				}
			}
			else
			{
                if (!hasDoubleJumped && Input.GetButtonDown("Jump"))
                {
                    hasDoubleJumped = true;
                    Jump();
					endTime = Time.time + jumpTime;
                }

                if (Time.time < endTime)
				{
                    Jump();
				}
                else if (rb.velocity.y <= 0.1f && !isOnWall)
				{
                    rb.AddForce(Vector3.down * fallForce * Time.deltaTime, ForceMode.Impulse);
                }
            }
            Debug.Log(isOnWall);
            if (isOnWall && Input.GetKey(KeyCode.LeftShift))
			{
                isMove = false;
                rb.useGravity = false;

                if (Input.GetButtonDown("Jump"))
				{
                    isMove = true;
                    rb.useGravity = true;
                    isOnWall = false;
                    Jump(wallNormal + transform.up);
				}
			}
			else
			{
                isMove = true;
                rb.useGravity = true;
                isOnWall = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
	{
		for (int i = 0; i < collision.contactCount; i++)
		{
            // we check that the contact point is below the player
            float dot = Vector3.Dot(-transform.up, transform.position - collision.GetContact(i).point);
            
            if (-dot >= groundBias)
			{
                isOnWall = false;
                isGrounded = true;
                break;
			}
            else if (dot < wallBias && dot > -wallBias)
            {
                wallNormal = collision.GetContact(i).normal;
                isOnWall = true;
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

    void Jump()
	{
        rb.AddForce(Vector3.up * playerJumpForce * Time.deltaTime, ForceMode.Impulse);
	}
    void Jump(Vector3 direction)
	{
        rb.AddForce(direction * playerJumpForce * 2 * Time.deltaTime, ForceMode.Impulse);
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