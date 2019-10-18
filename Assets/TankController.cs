using System;
using UnityEngine;
using static UnityEngine.ForceMode;

public class TankController : Controllable {
	public float trackPower = 1.0f;

	public WheelCollider[] leftTrack;
	public WheelCollider[] rightTrack;

	public Vector3 cameraOffset = -Vector3.forward;

	private void FixedUpdate() {
		var halfTorque = trackPower / 2;
		var brakeTorque = (1 - Math.Abs(forward)) * (1 - Math.Abs(side)) * halfTorque;
		foreach (var wheel in leftTrack) {
			wheel.motorTorque = forward * trackPower + side * halfTorque;
			wheel.brakeTorque = brakeTorque;
		}

		foreach (var wheel in rightTrack) {
			wheel.motorTorque = forward * trackPower - side * halfTorque;
			wheel.brakeTorque = brakeTorque;
		}
	}


	public override void SetCamera(Camera followCamera) {
		var cameraTransform = followCamera.transform;
		cameraTransform.SetParent(transform, false);
		cameraTransform.localPosition = cameraOffset;
		cameraTransform.LookAt(transform);
	}
}
