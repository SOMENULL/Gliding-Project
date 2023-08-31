using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Transform PlayerTransform;

    [SerializeField] private float SmoothTime;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float MaxViewRange;
    private float mouseX, mouseY;

    [SerializeField] private Vector3 Offset;
    private Vector3 CurrentVelocity;

    private void FixedUpdate()
    {
        FollowTargetTransform();
    }
    private void Update()
    {
        CameraRotation();
    }

    private void FollowTargetTransform()
    {
        Vector3 desiredPosition = PlayerTransform.position + Offset;
        Vector3 positionInterpolation = Vector3.SmoothDamp(transform.position, desiredPosition, ref CurrentVelocity, SmoothTime);

        transform.position = positionInterpolation;
    }
    private void CameraRotation()
    {
        mouseX += Input.GetAxisRaw("Mouse Y") * Sensitivity;
        mouseY += Input.GetAxisRaw("Mouse X") * Sensitivity;
        float clampedX = Mathf.Clamp(mouseX, -MaxViewRange, MaxViewRange);

        Quaternion targetRotation = Quaternion.Euler(clampedX, mouseY, transform.eulerAngles.z);
        transform.rotation = targetRotation;
    }
}
