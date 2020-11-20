using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public float speed = 10;

    [HideInInspector]
    public float lifeTime;
    float distanceTravelled;

    Vector3 startPos;

    // Start is called before the first frame update
    void OnEnable()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled = Mathf.Abs((startPos - transform.position).magnitude);

        if (distanceTravelled <= lifeTime)
		{
            transform.Translate(Vector3.forward * speed * Time.deltaTime * 10, Space.Self);
		}
    }
}
