using Mirror;
using UnityEngine;
using static UnityEngine.ForceMode;
using static System.Math;
using static Mirror.NetworkServer;
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

	public Vector3 cameraOffset = -forward;

	private float _shotDelta;

	private void Update() {
		_yaw += controls.mouseX * horizontalSpeed;
		_pitch = Max(minPitch, Min(maxPitch, _pitch + controls.mouseY * verticalSpeed));
		head.localRotation = AngleAxis(_yaw, up);
		gun.localRotation = AngleAxis(_pitch, left);

		_shotDelta = Min(cooldown, _shotDelta + Time.deltaTime);

		if (!controls.fire1 || !(_shotDelta >= cooldown)) return;
		CmdShoot();
		_shotDelta = 0.0f;
	}

	[Command]
	private void CmdShoot() {
		var shot = Instantiate(shotType, muzzle.transform.position, muzzle.transform.rotation);
		SpawnWithClientAuthority(shot, player.connectionToClient);
	}

	public override void SetCamera(Camera followCamera) {
		var cameraTransform = followCamera.transform;
		cameraTransform.SetParent(gun, false);
		cameraTransform.localPosition = cameraOffset;
		cameraTransform.LookAt(gun);
	}
}
