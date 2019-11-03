using UnityEngine;
using static Buildings.Utils;

public class Unbuildable : MonoBehaviour {
	public Bounds bounds { get; private set; }
	private GameController _game;

	private void Start() {
		_game = FindObjectOfType<GameController>();
		_game.AddUnbuildable(this);
		bounds = GetMaxBounds(gameObject);
	}

	private void OnDestroy() {
		if (_game) _game.RemoveUnbuildable(this);
	}
}
