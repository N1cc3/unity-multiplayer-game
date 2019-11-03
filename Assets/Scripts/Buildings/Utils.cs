using UnityEngine;

namespace Buildings {
	public static class Utils {
		public static Bounds GetMaxBounds(GameObject g) {
			var b = new Bounds(g.transform.position, Vector3.zero);
			foreach (var r in g.GetComponentsInChildren<Collider>()) b.Encapsulate(r.bounds);
			return b;
		}
	}
}
