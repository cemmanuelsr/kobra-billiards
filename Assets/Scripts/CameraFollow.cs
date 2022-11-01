using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetOffset;
    public float linearSpeed;
    public float rotationalSpeed;
    void Start()
    {
        targetOffset = new Vector3(0.0f, 0.5f, 0.25f);
        linearSpeed = 50.0f;
        rotationalSpeed = 50.0f;
    }

    void LateUpdate()
    {
        Vector3 newPosition = target.transform.position + (target.transform.forward * targetOffset.z) + (target.transform.up * targetOffset.y);
        newPosition = Vector3.Slerp(transform.position, newPosition, Time.smoothDeltaTime * linearSpeed);
        transform.position = newPosition;
        transform.LookAt(target.transform);
    }

    public void setNewTarget(GameObject newTarget) {
        target = newTarget;
    }

    public void setNewOffset(Vector3 newOffset) {
        targetOffset = newOffset;
    }
}
