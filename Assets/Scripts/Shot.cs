using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

public class Shot : NetworkBehaviour {
	public float lifetime;

	private float _timeSpawned;
	private int _ricochets;

	private void Start() {
		if (isServer) {
			_timeSpawned = Time.time;
			_ricochets = Random.value > 0.5 ? 1 : 0;
		} else {
			var rb = GetComponent<Rigidbody>();
			rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			rb.isKinematic = true;
		}
	}

	private void Update() {
		if (!isServer) return;
		if (Time.time - _timeSpawned > lifetime) Remove();
	}

	private void OnCollisionEnter(Collision other) {
		if (!isServer) return;
		if (_ricochets-- <= 0) Remove();
	}

	[Server]
	private void Remove() {
		Destroy(gameObject);
		NetworkServer.Destroy(gameObject);
	}
}
