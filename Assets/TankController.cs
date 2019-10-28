using System;
using UnityEngine;

public class TankController : TurretControl {
	public float trackPower = 1.0f;

	public WheelCollider[] leftTrack;
	public WheelCollider[] rightTrack;

	private void FixedUpdate() {
		var halfTorque = trackPower / 2;
		var brakeTorque = (1 - Math.Abs(controls.forward)) * (1 - Math.Abs(controls.side)) * halfTorque;
		foreach (var wheel in leftTrack) {
			wheel.motorTorque = controls.forward * trackPower + controls.side * halfTorque;
			wheel.brakeTorque = brakeTorque;
		}

		foreach (var wheel in rightTrack) {
			wheel.motorTorque = controls.forward * trackPower - controls.side * halfTorque;
			wheel.brakeTorque = brakeTorque;
		}
	}
}
