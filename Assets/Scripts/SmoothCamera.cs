using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    public float minX = -5.24f;
    public float minY = -4.22f;
    public float maxY = 4.6f;
    public float maxX = 71.79f;
    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 8f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
        if (transform.position.x <= minX)
            transform.position = new Vector3(minX,transform.position.y,transform.position.z);
        if (transform.position.y >= maxY)
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        if (transform.position.y <= minY)
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        if (transform.position.y >= maxY)
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
    }
}
