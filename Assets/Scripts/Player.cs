using System.Collections.Generic;
using Mirror;
using UnityEngine;
using static System.Array;
using static System.Math;
using static System.Single;
using static Controllable;
using static GameController;
using static GameController.BuildingType;
using static UnityEngine.CursorLockMode;
using static UnityEngine.Input;
using static UnityEngine.Physics;
using static UnityEngine.Quaternion;
using static UnityEngine.Vector3;

public class Player : NetworkBehaviour {
	public float moveSpeed = 1.0f;
	public float rotateSpeed = 1.0f;

	private float _yaw;
	private float _pitch;
	private Controllable _vehicleCtrl;

	private GameController _game;
	private Build _build;
	private Spawn _spawn;
	private TerrainCollider _terrainCollider;

	private GameObject _currentHolo;
	private BuildingType _currentHoloType;

	private readonly RaycastHit[] _hits = new RaycastHit[10];
	private readonly Dictionary<Renderer, Color> _originalColors = new Dictionary<Renderer, Color>();
	private Controls _controls;

	private void Awake() {
		_game = FindObjectOfType<GameController>();
	}

	private void Start() {
		_build = GetComponent<Build>();
		_spawn = GetComponent<Spawn>();

		if (!isLocalPlayer) return;

		_terrainCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();

		Cursor.visible = false;
		Cursor.lockState = Locked;
		FindObjectOfType<NetworkManagerHUD>().showGUI = false;

		SetCamera(Camera.main);
		SetBuildMode(Headquarters);
	}

	private void SetBuildMode(BuildingType buildingType) {
		if (_currentHolo) return;
		_currentHolo = Instantiate(_game.GetBuildingHolo(buildingType));
		_currentHoloType = buildingType;
	}

	private void Update() {
		if (!isLocalPlayer) return;

		var newControls = new Controls(
			GetAxis("Vertical"),
			GetAxis("Horizontal"),
			GetAxis("Mouse X"),
			GetAxis("Mouse Y"),
			GetButton("Jump"),
			GetButton("Crouch"),
			GetButton("Fire1")
		);

		HandleHolo();
		HandleHighlights();

		if (_vehicleCtrl) {
			_vehicleCtrl.SetControls(newControls);
			if (GetButtonDown("Cancel")) CmdExitVehicle();
		} else {
			SetControls(newControls);
			if (GetButtonDown("Spawn1")) _spawn.CmdSpawn();
			if (GetButtonDown("Spawn2")) SetBuildMode(HmgTurret);
		}
	}

	private void FixedUpdate() {
		transform.Translate(-_controls.forward * moveSpeed * forward);
		transform.Translate(-_controls.side * moveSpeed * right);
		_yaw += _controls.mouseX * rotateSpeed;
		_pitch = Max(-90, Min(90, _pitch + _controls.mouseY * rotateSpeed));
		transform.rotation = AngleAxis(_yaw, up) * AngleAxis(_pitch, right);
		if (_controls.jump) transform.Translate(moveSpeed * up);
		if (_controls.crouch) transform.Translate(moveSpeed * down);
	}

	private void SetControls(Controls newControls) {
		_controls = newControls;
	}

	private void ResetControls() {
		_controls = new Controls();
	}

	private void SetCamera(Component followCamera) {
		var cameraTransform = followCamera.transform;
		cameraTransform.SetParent(transform, false);
		cameraTransform.localPosition = 0.01f * forward;
		cameraTransform.rotation = identity;
		cameraTransform.LookAt(transform);
	}

	private void HandleHighlights() {
		foreach (var pair in _originalColors) pair.Key.material.color = pair.Value;
		_originalColors.Clear();

		if (_currentHolo || _vehicleCtrl) return;
		var t = transform;
		Clear(_hits, 0, _hits.Length);
		RaycastNonAlloc(new Ray(t.position, -t.forward), _hits, PositiveInfinity);

		foreach (var hit in _hits) {
			if (!hit.transform) continue;
			var controllable = hit.transform.GetComponent<Controllable>();
			if (!controllable) controllable = hit.transform.GetComponentInParent<Controllable>();
			if (!controllable) continue;
			var renderers = controllable.GetComponentsInChildren<Renderer>();
			foreach (var r in renderers) {
				if (!_originalColors.ContainsKey(r)) _originalColors.Add(r, r.material.color);
				r.material.color = Color.cyan;
			}

			if (_controls.fire1) CmdEnterVehicle(controllable.gameObject.GetComponent<NetworkIdentity>().netId);

			break; // What if it wasn't the closest controllable?
		}
	}

	[Command]
	private void CmdExitVehicle() {
		var vehicle = _vehicleCtrl.gameObject;
		vehicle.GetComponent<NetworkIdentity>().RemoveClientAuthority();
		SetKinematic(vehicle, false);
		_vehicleCtrl.ResetControls();
		_vehicleCtrl = null;
		TargetClearVehicle();
	}

	[TargetRpc]
	private void TargetClearVehicle() {
		SetCamera(Camera.main);
		if (isServer) return;
		SetKinematic(_vehicleCtrl.gameObject, false);
		_vehicleCtrl = null;
	}

	[Command]
	private void CmdEnterVehicle(uint vehicleNetId) {
		var vehicle = NetworkIdentity.spawned[vehicleNetId].gameObject;
		if (vehicle.GetComponent<Owned>().owner != connectionToClient.connectionId) return;
		SetKinematic(vehicle, true);
		vehicle.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
		_vehicleCtrl = vehicle.GetComponent<Controllable>();
		TargetSetVehicle(vehicleNetId);
	}

	[TargetRpc]
	private void TargetSetVehicle(uint vehicleNetId) {
		ResetControls();
		var vehicle = NetworkIdentity.spawned[vehicleNetId].gameObject;
		_vehicleCtrl = vehicle.GetComponent<Controllable>();
		_vehicleCtrl.SetCamera(Camera.main);
		SetKinematic(vehicle, false);
	}

	private void HandleHolo() {
		if (!_currentHolo) return;

		var t = transform;
		_terrainCollider.Raycast(new Ray(t.position, -t.forward), out var hit, PositiveInfinity);
		_currentHolo.transform.position = hit.point;

		if (!_controls.fire1) return;
		var buildingHolo = _currentHolo.GetComponent<BuildingHolo>();
		if (!buildingHolo.CanBeBuilt()) return;

		_build.CmdBuild(_currentHolo.transform.position, _currentHoloType);
		Destroy(_currentHolo);
		_currentHolo = null;
	}

	private static void SetKinematic(GameObject vehicle, bool isOn) {
		var rb = vehicle.gameObject.GetComponent<Rigidbody>();
		if (rb) rb.isKinematic = isOn;
	}
}
