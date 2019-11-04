using System.Linq;
using UnityEngine;

public class BuildingHolo : MonoBehaviour {
	private static readonly Color Green = new Color(0, 0.2f, 0);
	private static readonly Color Red = new Color(0.2f, 0, 0);
	private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

	private GameController _game;
	private MeshRenderer[] _holos;
	private Unbuildable _unbuildable;

	private void Awake() {
		_game = FindObjectOfType<GameController>();
		_holos = GetComponentsInChildren<MeshRenderer>();
		_unbuildable = GetComponentInChildren<Unbuildable>();
	}

	private void LateUpdate() {
		var color = CanBeBuilt() ? Green : Red;
		foreach (var holo in _holos) holo.material.SetColor(EmissionColor, color);
	}

	public bool CanBeBuilt() {
		return !_game.Unbuildables().Any(unbuildable => unbuildable.box.bounds.Intersects(_unbuildable.box.bounds));
	}
}
