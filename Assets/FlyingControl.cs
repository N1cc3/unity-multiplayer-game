using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.ForceMode;

public class FlyingControl : NetworkBehaviour
{
    public float thrustSpeed = 10.0f;
    public float yawSpeed = 1.0f;
    public float pitchSpeed = 1.0f;
    public float rollSpeed = 1.0f;
    public float jumpSpeed = 8.0f;
    public float defaultJumpThrust = 5f;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        var thrust = thrustSpeed * Input.GetAxis("Vertical");
        var yaw = yawSpeed * Input.GetAxis("Horizontal");
        var pitch = pitchSpeed * Input.GetAxis("Mouse Y");
        var roll = rollSpeed * Input.GetAxis("Mouse X");
        var isJumping = Input.GetButton("Jump");
        var isLanding = Input.GetButton("Crouch");

        _rb.AddForce(-thrust * transform.forward, Acceleration);
        _rb.AddTorque(yaw * transform.up, Acceleration);
        _rb.AddTorque(pitch * transform.right, Acceleration);
        _rb.AddTorque(roll * transform.forward, Acceleration);
        if (isJumping) _rb.AddForce(jumpSpeed * transform.up, Acceleration);

        // Default jump thrust
        if (!isLanding) _rb.AddForce(defaultJumpThrust * transform.up, Acceleration);
    }
}
