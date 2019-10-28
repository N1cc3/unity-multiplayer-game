using UnityEngine;
using static UnityEngine.ForceMode;

public class FlyingControl : Controllable {
	public float thrustSpeed = 8.0f;
	public float yawSpeed = 1.0f;
	public float pitchSpeed = 1.0f;
	public float rollSpeed = 1.0f;
	public float jumpSpeed = 8.0f;
	public float defaultJumpThrust = 6f;

	public Vector3 cameraOffset = Vector3.forward;

	private Rigidbody _rb;
	private bool _isCrouching;

	private void Start() {
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		var t = transform;
		var tForward = t.forward;
		var tUp = t.up;

		_rb.AddForce(-controls.forward * thrustSpeed * tForward, Acceleration);
		_rb.AddTorque(controls.side * yawSpeed * tUp, Acceleration);
		_rb.AddTorque(controls.mouseX * rollSpeed * tForward, Acceleration);
		_rb.AddTorque(controls.mouseY * pitchSpeed * t.right, Acceleration);
		if (controls.jump) _rb.AddForce(jumpSpeed * tUp, Acceleration);
		if (!controls.crouch) _rb.AddForce(defaultJumpThrust * tUp, Acceleration);
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
