using UnityEngine;
using Mirror;
using static UnityEngine.ForceMode;
using Random = UnityEngine.Random;

public class Shot : NetworkBehaviour {
	public float lifetime;
	public float muzzleVelocity;

	private float _timeSpawned;
	private int _ricochets;

	private void Start() {
		if (hasAuthority) return;
		var rb = GetComponent<Rigidbody>();
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		rb.isKinematic = true;
	}

	public override void OnStartAuthority() {
		base.OnStartAuthority();
		var rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		_timeSpawned = Time.time;
		_ricochets = Random.value > 0.5 ? 1 : 0;
		GetComponent<Rigidbody>().AddForce(muzzleVelocity * transform.forward, VelocityChange);
	}

	private void Update() {
		if (!hasAuthority) return;
		if (Time.time - _timeSpawned > lifetime) Remove();
	}

	private void OnCollisionEnter(Collision other) {
		if (!hasAuthority) return;
		if (_ricochets-- <= 0) Remove();
	}

	private void Remove() {
		NetworkServer.Destroy(gameObject);
	}
}
