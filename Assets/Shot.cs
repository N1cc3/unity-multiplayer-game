using UnityEngine;
using Random = UnityEngine.Random;

public class Shot : MonoBehaviour {
	public float lifetime;

	private float _timeSpawned;
	private int _ricochets;

	private void Start() {
		_timeSpawned = Time.time;
		_ricochets = Random.value > 0.5 ? 1 : 0;
	}

	private void Update() {
		if (Time.time - _timeSpawned > lifetime) Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other) {
		if (_ricochets-- <= 0) Destroy(gameObject);
	}
}
