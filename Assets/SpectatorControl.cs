using System.Collections.Generic;
using UnityEngine;
using static System.Array;
using static System.Math;
using static System.Single;
using static Mirror.NetworkServer;
using static UnityEngine.Physics;
using static UnityEngine.Quaternion;
using static UnityEngine.Vector3;

public class SpectatorControl : Controllable {
	public float moveSpeed = 1.0f;
	public float rotateSpeed = 1.0f;

	private float _yaw;
	private float _pitch;

//	public PlayerControl playerControl;

	private Game _game;
	private TerrainCollider _terrainCollider;

	private GameObject _headquartersHoloPrefab;
	private GameObject _turretHoloPrefab;
	private GameObject _currentHolo;

	private readonly RaycastHit[] _hits = new RaycastHit[10];
	private readonly Dictionary<Renderer, Color> _originalColors = new Dictionary<Renderer, Color>();

	private void Awake() {
		_headquartersHoloPrefab = Resources.Load("headquarters_holo") as GameObject;
		_turretHoloPrefab = Resources.Load("hmg_turret_holo") as GameObject;
	}

	public void Init() {
		_game = FindObjectOfType<Game>();
		_terrainCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
		_currentHolo = Instantiate(_headquartersHoloPrefab);
	}

	private void Update() {
		HandleHolo();
		HandleHighlights();
	}

	private void HandleHighlights() {
		foreach (var pair in _originalColors) pair.Key.material.color = pair.Value;
		_originalColors.Clear();

		if (_currentHolo) return; // || !playerControl.inSpectatorMode) return;
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

//			if (fire1) playerControl.SetControlledObject(controllable.gameObject);

			break; // What if it wasn't the closest controllable?
		}
	}

	private void HandleHolo() {
		if (!_currentHolo) return;

		var t = transform;
		_terrainCollider.Raycast(new Ray(t.position, -t.forward), out var hit, PositiveInfinity);
		_currentHolo.transform.position = hit.point;

		if (!controls.fire1) return;
		var buildingHolo = _currentHolo.GetComponent<BuildingHolo>();
		if (!buildingHolo.CanBeBuilt()) return;

		var building = Instantiate(buildingHolo.building, _currentHolo.transform.position,
			_currentHolo.transform.rotation);
		Spawn(building);
//		_game.Unbuildables.Add(building.GetComponentInChildren<Unbuildable>());
		Destroy(_currentHolo);
		_currentHolo = null;

	}

	private void FixedUpdate() {
		transform.Translate(-controls.forward * moveSpeed * forward);
		transform.Translate(-controls.side * moveSpeed * right);
		_yaw += controls.mouseX * rotateSpeed;
		_pitch = Max(-90, Min(90, _pitch + controls.mouseY * rotateSpeed));
		transform.rotation = AngleAxis(_yaw, up) * AngleAxis(_pitch, right);
		if (controls.jump) transform.Translate(moveSpeed * up);
		if (controls.crouch) transform.Translate(moveSpeed * down);
	}

	public override void SetCamera(Camera followCamera) {
		var cameraTransform = followCamera.transform;
		cameraTransform.SetParent(transform, false);
		cameraTransform.localPosition = 0.01f * forward;
		cameraTransform.LookAt(transform);
	}

	public void BuildTurret() {
		if (_currentHolo) return;
		_currentHolo = Instantiate(_turretHoloPrefab);
	}
}
