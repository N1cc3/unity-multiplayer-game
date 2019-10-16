using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using static UnityEngine.ForceMode;

public class FlyingControl : Controllable
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

    private void FixedUpdate()
    {
        var t = transform;
        var tForward = t.forward;
        var tUp = t.up;

        _rb.AddForce(-Forward * thrustSpeed * tForward, Acceleration);
        _rb.AddTorque(Side * yawSpeed * tUp, Acceleration);
        _rb.AddTorque(MouseX * rollSpeed * tForward, Acceleration);
        _rb.AddTorque(MouseY * pitchSpeed * t.right, Acceleration);
        if (Jump) _rb.AddForce(jumpSpeed * tUp, Acceleration);
        if (!Crouch) _rb.AddForce(defaultJumpThrust * tUp, Acceleration);
    }


    public override void SetCamera(Camera followCamera)
    {
        var cameraTransform = followCamera.transform;
        cameraTransform.SetParent(transform, false);
        cameraTransform.localPosition = cameraOffset;
        cameraTransform.LookAt(transform);
    }
}
