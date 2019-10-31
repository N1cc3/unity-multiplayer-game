using System;
using UnityEngine;

public class Unbuildable : MonoBehaviour {
	public Collider area;

	private Game _game;

	private void Start() {
		_game = FindObjectOfType<Game>();
		_game.AddUnbuildable(this);
	}

	private void OnDestroy() {
		_game.RemoveUnbuildable(this);
	}
}
