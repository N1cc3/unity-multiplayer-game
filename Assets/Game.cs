using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	private readonly HashSet<Unbuildable> _unbuildables = new HashSet<Unbuildable>();

	private void Start() {
		foreach (var unbuildable in FindObjectsOfType<Unbuildable>()) _unbuildables.Add(unbuildable);
	}

	public void AddUnbuildable(Unbuildable unbuildable) {
		_unbuildables.Add(unbuildable);
	}

	public void RemoveUnbuildable(Unbuildable unbuildable) {
		_unbuildables.Remove(unbuildable);
	}

	public HashSet<Unbuildable> Unbuildables() {
		return _unbuildables;
	}
}
