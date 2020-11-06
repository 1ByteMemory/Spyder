using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask layer;

    public float playerSpeed = 5f;
    public float playerJumpForce = 1f;
    public float fallForce = 10;
    public float jumpTime = 0.5f;
    public float jumpBoost = 0.001f;

    [Range(0,1)]
    public float groundBias = 0.9f;
    [Range(0, 1)]
    public float wallBias = 0;
    public CapsuleCollider wallCollider;

    [HideInInspector]
    public int jumps;
    public int maxJumps = 2;
	public bool JumpButtonHeld => Input.GetButton("Jump");
	public bool JumpButtonReleased => Input.GetButtonUp("Jump");
	public bool JumpButtonOnPress => Input.GetButtonDown("Jump");

	bool isOnWall = false;
    bool falling = false;
    bool isMove = true;
    Vector3 wallNormal;

    public float lookSpeed = 5f;
    
    public Transform fpsCamera;
    Rigidbody rb;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GroundChecker.layerMask = layer;
    }


    float endTime;


	private void Update()
	{
		if (!Cursor.visible)
		{
            Look();
        }
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        
        if (!Cursor.visible)
        {
            if (isMove)
			{
                Move();
			}

            Ray ray = new Ray(transform.position, -transform.up);
            bool isGrounded = GroundChecker.IsGrounded(ray, transform.lossyScale.y / 2 + 0.3f);

            if (isGrounded && !JumpButtonHeld)
			{
                jumps = 0;
			}

            if (isGrounded)
            {
                falling = false;
                if (JumpButtonHeld && jumps == 0)
				{
                    jumps++;
                    Jump();
				}
            }
            else
            {
                if (jumps < maxJumps && JumpButtonOnPress)
                {
                    falling = false;
                    jumps++;
                    Jump();
                }
                else if (rb.velocity.y <= -0.2f && !isOnWall && !falling)
                {
                    falling = true;
                    Jump(-transform.up, fallForce);
                }
            }

            if (isOnWall && Input.GetKey(KeyCode.LeftShift))
			{
                isMove = false;
                rb.useGravity = false;
                falling = false;

                if (JumpButtonOnPress)
				{
                    isMove = true;
                    rb.useGravity = true;
                    isOnWall = false;
                    Jump(wallNormal + transform.up, 1);
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
                break;
			}
            else if (dot < wallBias && dot > -wallBias)
            {
                wallNormal = collision.GetContact(i).normal;
                isOnWall = true;
                break;
            }
		}
	}



	void Move()
    {
        //---MOVE---//

        float axisV = Input.GetAxisRaw("Vertical");     // Forward/Backward Input
        float axisH = Input.GetAxisRaw("Horizontal");   // Left/Right Input

        Vector3 force = new Vector3(axisH, 0, axisV).normalized * playerSpeed * Time.fixedDeltaTime;

        //transform.Translate(new Vector3(axisH, 0, axisV).normalized * playerSpeed * Time.deltaTime);
        rb.AddForce(transform.TransformDirection(force), ForceMode.Impulse);
    }

    void Jump()
	{
        Jump(transform.up, 1);
	}
    void Jump(Vector3 direction, float multiplyer)
	{
        rb.AddForce(direction * playerJumpForce * multiplyer * Time.fixedDeltaTime, ForceMode.Impulse);
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