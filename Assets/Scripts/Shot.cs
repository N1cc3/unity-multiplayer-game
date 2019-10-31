using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class Shot : NetworkBehaviour {
	public float lifetime;

	private float _timeSpawned;
	private int _ricochets;

	private void Start() {
		_timeSpawned = Time.time;
		_ricochets = Random.value > 0.5 ? 1 : 0;
	}

	private void Update() {
		if (Time.time - _timeSpawned > lifetime) Remove();
	}

	private void OnCollisionEnter(Collision other) {
		if (_ricochets-- <= 0) Remove();
	}

	private void Remove() {
		Destroy(gameObject);
		NetworkServer.Destroy(gameObject);
	}
}
