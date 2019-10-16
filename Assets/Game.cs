using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	public readonly List<Unbuildable> Unbuildables = new List<Unbuildable>();

	private void Start() {
		Unbuildables.AddRange(FindObjectsOfType<Unbuildable>());
	}
}