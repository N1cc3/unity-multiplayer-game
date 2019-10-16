using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.ForceMode;

public class PlayerController : MonoBehaviour {
	public float horizontalSpeed = 1.0f;
	public float verticalSpeed = 1.0f;

	public GameObject horizontalTransform;
	public GameObject verticalTransform;

	public GameObject shotType;
	public GameObject muzzle;
	public float fireRate = 0.1f;
	public float muzzleImpulse = 0.5f;

	private float _shotDelta;

	private void Update() {
		var h = horizontalSpeed * Input.GetAxis("Mouse X");
		var v = verticalSpeed * Input.GetAxis("Mouse Y");
		horizontalTransform.transform.Rotate(Vector3.up, h);
		verticalTransform.transform.Rotate(Vector3.right, v);

		_shotDelta = Math.Min(fireRate, _shotDelta + Time.deltaTime);
		var isShooting = Input.GetButton("Fire1");
		if (isShooting && _shotDelta >= fireRate) Shoot();
	}

	private void Shoot() {
		var rotation = muzzle.transform.rotation;
		var shot = Instantiate(shotType, muzzle.transform.position, rotation);
		var shotDirection = rotation * Vector3.back;
		shot.GetComponent<Rigidbody>().AddForce(muzzleImpulse * shotDirection, Impulse);
		_shotDelta = 0.0f;
	}
}
