using UnityEngine;

public class Unbuildable : MonoBehaviour {
	public Collider area;

	private GameController _game;

	private void Start() {
		_game = FindObjectOfType<GameController>();
		_game.AddUnbuildable(this);
	}

	private void OnDestroy() {
		if (_game) _game.RemoveUnbuildable(this);
	}
}
