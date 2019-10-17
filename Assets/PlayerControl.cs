using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Input;

public class PlayerControl : NetworkBehaviour {
	public bool inSpectatorMode;

	private Controllable _controllable;
	private Transform _spawnPoint;

	private GameObject _spectatorPrefab;
	private GameObject _mediumTransportPrefab;
	private GameObject _spectator;

	private void Awake() {
		_spectatorPrefab = Resources.Load("spectator") as GameObject;
		_mediumTransportPrefab = Resources.Load("medium_transport") as GameObject;
	}

	public override void OnStartLocalPlayer() {
		_spectator = Instantiate(_spectatorPrefab);
		_spectator.GetComponent<SpectatorControl>().playerControl = this;
		SpectatorMode();
		_spawnPoint = GameObject.FindWithTag("Respawn").transform;
	}

	public void SetControlledObject(GameObject obj) {
		if (_controllable) _controllable.ResetControls();
		_controllable = obj.GetComponent<Controllable>();
		inSpectatorMode = obj == _spectator;
		_controllable.SetCamera(Camera.main);
	}

	private void Update() {
		if (!isLocalPlayer) return;
		if (GetButtonDown("Cancel")) SpectatorMode();
		if (GetButtonDown("Spawn1")) Spawn1();
		if (GetButtonDown("Spawn2")) Spawn2();

		if (_controllable == null) return;
		_controllable.forward = GetAxis("Vertical");
		_controllable.side = GetAxis("Horizontal");
		_controllable.mouseX = GetAxis("Mouse X");
		_controllable.mouseY = GetAxis("Mouse Y");
		_controllable.jump = GetButton("Jump");
		_controllable.crouch = GetButton("Crouch");
		_controllable.fire1 = GetButton("Fire1");
	}

	private void SpectatorMode() {
		var cameraT = Camera.main.transform;
		var cameraPosition = cameraT.position;
		_spectator.transform.position = cameraPosition;
		_spectator.transform.LookAt(cameraT.rotation * -Vector3.forward + cameraPosition);
		SetControlledObject(_spectator);
	}

	private void Spawn1() {
		if (!inSpectatorMode) return;
		var mediumTransport = Instantiate(_mediumTransportPrefab, _spawnPoint.position, _spawnPoint.rotation);
		SetControlledObject(mediumTransport);
	}

	private void Spawn2() {
		if (!inSpectatorMode) return;
		_spectator.GetComponent<SpectatorControl>().BuildTurret();
	}
}
