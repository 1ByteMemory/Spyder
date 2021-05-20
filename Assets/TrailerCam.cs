using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCam : MonoBehaviour
{

    public float lookSpeed = 10;
    public float moveSpeed = 5;


    private Rigidbody rb;
	public void Start()
	{
        rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
    {
        float updown = 0;
        if (Input.GetKey(KeyCode.Space)) updown = 1;
        if (Input.GetKey(KeyCode.LeftShift)) updown = -1;
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), updown, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 m = transform.TransformDirection(move);
        rb.AddForce(m * moveSpeed * Time.deltaTime, ForceMode.Impulse);

		
        Vector3 look = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));

        transform.eulerAngles += look * lookSpeed * Time.deltaTime;
        
    }
}
