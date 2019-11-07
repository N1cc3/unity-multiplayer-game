using System;
using UnityEngine;
using static UnityEngine.ForceMode;

public class BuggyControl : Controllable {
	public float torque = 1.0f;

	public Vector3 cameraOffset = Vector3.back;

	public WheelCollider[] leftWheels;
	public WheelCollider[] rightWheels;

	private Rigidbody _rb;

	private void Start() {
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		var halfTorque = torque / 2;
//		var brakeTorque = (1 - Math.Abs(controls.forward)) * (1 - Math.Abs(controls.side)) * halfTorque;
		foreach (var wheel in leftWheels) {
			wheel.motorTorque = controls.forward * torque + controls.side * halfTorque;
//			wheel.brakeTorque = brakeTorque;
		}

		foreach (var wheel in rightWheels) {
			wheel.motorTorque = controls.forward * torque - controls.side * halfTorque;
//			wheel.brakeTorque = brakeTorque;
		}

		_rb.AddForce(controls.forward * 10 * transform.forward, Acceleration);
	}

	public override void SetCamera(Camera followCamera) {
		var parent = transform;
		var cameraTransform = followCamera.transform;
		cameraTransform.SetParent(parent, false);
		cameraTransform.localPosition = cameraOffset;
		cameraTransform.rotation = parent.rotation;
		cameraTransform.LookAt(parent, parent.up);
	}
}
   Assets/Import/scavbuggy/scavbuggy.blend Assets/Import/scavbuggy/scavbuggy.blend.meta Assets/Prefabs/Resources/scavbuggy.prefab Assets/Prefabs/Resources/scavbuggy.prefab.meta Assets/Prefabs/Resources/tank.prefab Assets/Scenes/Island.unity Assets/Scripts/Controllables/BuggyControl.cs Assets/Scripts/GameController.cs Assets/Scripts/Player.cs ProjectSettings/InputManager.asset
