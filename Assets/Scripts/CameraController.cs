using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (target == null) return;
        if(!Mathf.Approximately(transform.rotation.eulerAngles.x, 70f))
        {
            Quaternion targetRotation = Quaternion.Euler(70f, 0f, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 4f * Time.deltaTime);
        }
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
