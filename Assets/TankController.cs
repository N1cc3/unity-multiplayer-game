using System;
using UnityEngine;

public class TankController : TurretControl {
	public float trackPower = 1.0f;

	public WheelCollider[] leftTrack;
	public WheelCollider[] rightTrack;

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
}
