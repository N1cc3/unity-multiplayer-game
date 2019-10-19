using UnityEngine;
using static UnityEngine.ForceMode;
using static System.Math;
using static UnityEngine.Quaternion;
using static UnityEngine.Vector3;

public class TurretControl : Controllable {
	public float horizontalSpeed = 1.0f;
	public float verticalSpeed = 1.0f;

	public float minPitch = -90.0f;
	public float maxPitch = 90.0f;

	private float _yaw;
	private float _pitch;

	public Transform head;
	public Transform gun;

	public GameObject shotType;
	public GameObject muzzle;
	public float cooldown = 0.1f;
	public float muzzleVelocity = 50.0f;

	public Vector3 cameraOffset = Vector3.forward;

	private float _shotDelta;

	private void Update() {
		_yaw += mouseX * horizontalSpeed;
		_pitch = Max(minPitch, Min(maxPitch, _pitch + mouseY * verticalSpeed));
		head.localRotation = AngleAxis(_yaw, up);
		gun.localRotation = AngleAxis(_pitch, left);

		_shotDelta = Min(cooldown, _shotDelta + Time.deltaTime);
		if (fire1 && _shotDelta >= cooldown) Shoot();
	}

	private void Shoot() {
		var rotation = muzzle.transform.rotation;
		var shot = Instantiate(shotType, muzzle.transform.position, rotation);
		var shotDirection = rotation * Vector3.forward;
		shot.GetComponent<Rigidbody>().AddForce(muzzleVelocity * shotDirection, VelocityChange);
		_shotDelta = 0.0f;
	}

	public override void SetCamera(Camera followCamera) {
		var cameraTransform = followCamera.transform;
		cameraTransform.SetParent(gun, false);
		cameraTransform.localPosition = cameraOffset;
		cameraTransform.LookAt(gun);
	}
}
