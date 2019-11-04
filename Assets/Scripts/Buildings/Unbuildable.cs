using UnityEngine;

public class Unbuildable : MonoBehaviour {
	public BoxCollider box;
	private GameController _game;

	private void Start() {
		if (!isActiveAndEnabled) return;
		_game = FindObjectOfType<GameController>();
		_game.AddUnbuildable(this);
	}

	private void OnDestroy() {
		if (_game) _game.RemoveUnbuildable(this);
	}
}
