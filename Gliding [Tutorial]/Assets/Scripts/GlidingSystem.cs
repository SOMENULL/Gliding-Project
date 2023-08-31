using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlidingSystem : MonoBehaviour
{
    [SerializeField] private float BaseSpeed = 30f;
    [SerializeField] private float MaxThrustSpeed;
    [SerializeField] private float MinThrustSpeed;
    [SerializeField] private float ThrustFactor;
    [SerializeField] private float DragFactor;
    [SerializeField] private float MinDrag;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float TiltStrength = 90;
    [SerializeField] private float LowPercent = 0.1f, HighPercent = 1;
    
    private float CurrentThrustSpeed;
    private float TiltValue, LerpValue;

    private Transform CameraTransform;
    private Rigidbody Rb;

    private void Start()
    {
        CameraTransform = Camera.main.transform.parent;
        Rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GlidingMovement();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        ManageRotation();
    }

    private void GlidingMovement() 
    {
        float pitchInDeg = transform.eulerAngles.x % 360;
        float pitchInRads = transform.eulerAngles.x * Mathf.Deg2Rad;
        float mappedPitch = -Mathf.Sin(pitchInRads);
        float offsetMappedPitch = Mathf.Cos(pitchInRads) * DragFactor;
        float accelerationPercent = pitchInDeg >= 300f ? LowPercent : HighPercent; 
        Vector3 glidingForce = -Vector3.forward * CurrentThrustSpeed;

        CurrentThrustSpeed += mappedPitch * accelerationPercent * ThrustFactor * Time.deltaTime;
        CurrentThrustSpeed = Mathf.Clamp(CurrentThrustSpeed, 0, MaxThrustSpeed);

        if (Rb.velocity.magnitude >= MinThrustSpeed)
        {
            Rb.AddRelativeForce(glidingForce);
            Rb.drag = Mathf.Clamp(offsetMappedPitch, MinDrag, DragFactor);
        }
        else 
        {
            CurrentThrustSpeed = 0;
        }

        Debug.Log(CurrentThrustSpeed);
    }
    private void ManageRotation() 
    {
        float mouseX = Input.GetAxis("Mouse X");
        TiltValue += mouseX * TiltStrength;

        if (mouseX == 0)
        {
            TiltValue = Mathf.Lerp(TiltValue, 0, LerpValue);
            LerpValue += Time.deltaTime;
        }
        else 
        {
            LerpValue = 0;
        }

        Quaternion targetRotation = Quaternion.Euler(CameraTransform.eulerAngles.x, CameraTransform.eulerAngles.y, TiltValue);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
    }
}
