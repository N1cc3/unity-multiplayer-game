using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingHolo : MonoBehaviour {
	private static readonly Color Green = new Color(0, 0.2f, 0);
	private static readonly Color Red = new Color(0.2f, 0, 0);
	private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

	public BoxCollider area;
	public GameObject building;

	private Game _game;

	private MeshRenderer[] _holos;

	private void Awake() {
		_game = FindObjectOfType<Game>();
		_holos = GetComponentsInChildren<MeshRenderer>();
	}

	private void LateUpdate() {
		var color = CanBeBuilt() ? Green : Red;
		foreach (var holo in _holos) holo.material.SetColor(EmissionColor, color);
	}

	public bool CanBeBuilt() {
		return !_game.Unbuildables().Any(unbuildable => unbuildable.area.bounds.Intersects(area.bounds));
	}
}
