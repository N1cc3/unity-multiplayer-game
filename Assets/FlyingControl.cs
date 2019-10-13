using System;
using UnityEngine;
using static UnityEngine.ForceMode;

public class FlyingControl : MonoBehaviour, IControllable
{
    public float thrustSpeed = 8.0f;
    public float yawSpeed = 1.0f;
    public float pitchSpeed = 1.0f;
    public float rollSpeed = 1.0f;
    public float jumpSpeed = 8.0f;
    public float defaultJumpThrust = 6f;

    public Vector3 cameraOffset = Vector3.forward;

    private Rigidbody _rb;
    private bool _isCrouching;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isCrouching) _rb.AddForce(defaultJumpThrust * transform.up, Acceleration);
        _isCrouching = false;
    }

    public void Forward(float amount)
    {
        _rb.AddForce(-1 * thrustSpeed * amount * transform.forward, Acceleration);
    }

    public void Side(float amount)
    {
        _rb.AddTorque(yawSpeed * amount * transform.up, Acceleration);
    }

    public void MouseX(float amount)
    {
        _rb.AddTorque(rollSpeed * amount * transform.forward, Acceleration);
    }

    public void MouseY(float amount)
    {
        _rb.AddTorque(pitchSpeed * amount * transform.right, Acceleration);
    }

    public void Jump()
    {
        _rb.AddForce(jumpSpeed * transform.up, Acceleration);
    }

    public void Crouch()
    {
        _isCrouching = true;
    }

    public void SetCamera(Camera followCamera)
    {
        var cameraTransform = followCamera.transform;
        cameraTransform.SetParent(transform, false);
        cameraTransform.localPosition = cameraOffset;
        cameraTransform.LookAt(transform);
    }
}
