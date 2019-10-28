using System;
using UnityEngine;
using Mirror;
using static Controllable;
using static Mirror.NetworkServer;
using static UnityEngine.Input;

public class PlayerControl : NetworkBehaviour {
	public bool inSpectatorMode;

	private Controllable _controllable;
	private Transform _spawnPoint;

	private GameObject _spectatorPrefab;
	private GameObject _spectator;

	private GameObject _mediumTransportPrefab;
	private GameObject _tankPrefab;

	private void Awake() {
		_spectatorPrefab = Resources.Load("spectator") as GameObject;
		_mediumTransportPrefab = Resources.Load("medium_transport") as GameObject;
		_tankPrefab = Resources.Load("tank") as GameObject;
	}

	private void Start() {
		if (!isServer) return;
		_spectator = Instantiate(_spectatorPrefab);
		Spawn(_spectator);
		var spectatorControl = _spectator.GetComponent<SpectatorControl>();
		spectatorControl.Init();
		SpectatorMode();
	}

	private void Update() {
		if (!isLocalPlayer) return;
		if (GetButtonDown("Cancel")) SpectatorMode();

		if (GetButtonDown("Spawn1")) CmdSpawn1();
		if (GetButtonDown("Spawn2")) Spawn2();
		if (GetButtonDown("Spawn3")) CmdSpawn3();

		CmdSetControls(new Controls(
			GetAxis("Vertical"),
			GetAxis("Horizontal"),
			GetAxis("Mouse X"),
			GetAxis("Mouse Y"),
			GetButton("Jump"),
			GetButton("Crouch"),
			GetButton("Fire1")
		));
	}

	[Command]
	private void CmdSetControls(Controls controls) {
		_controllable.SetControls(controls);
	}

	private void SpectatorMode() {
		var cameraT = Camera.main.transform;
		var cameraPosition = cameraT.position;
		_spectator.transform.position = cameraPosition;
		_spectator.transform.LookAt(cameraT.rotation * -Vector3.forward + cameraPosition);
		CmdSpectatorMode();
	}

	[Command]
	private void CmdSpectatorMode() {
		SetControlledObject(_spectator);
	}

	[Command]
	private void CmdSpawn1() {
		if (!inSpectatorMode) return;
		_spawnPoint = GameObject.FindWithTag("Respawn").transform;
		var mediumTransport = Instantiate(_mediumTransportPrefab, _spawnPoint.position, _spawnPoint.rotation);
		Spawn(mediumTransport);
		SetControlledObject(mediumTransport);
	}

	private void Spawn2() {
		if (!inSpectatorMode) return;
		_spectator.GetComponent<SpectatorControl>().BuildTurret();
	}

	[Command]
	private void CmdSpawn3() {
		if (!inSpectatorMode) return;
		_spawnPoint = GameObject.FindWithTag("Respawn").transform;
		var tank = Instantiate(_tankPrefab, _spawnPoint.position, _spawnPoint.rotation);
		Spawn(tank);
		SetControlledObject(tank);
	}

	[Server]
	private void SetControlledObject(GameObject obj) {
		if (_controllable) _controllable.ResetControls();
		_controllable = obj.GetComponent<Controllable>();
		_controllable.SetCamera(Camera.main);
		inSpectatorMode = obj == _spectator;
	}
}
