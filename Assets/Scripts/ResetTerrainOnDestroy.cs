using UnityEngine;

public class ResetTerrainOnDestroy : MonoBehaviour {
	public Terrain terrain;

	private float[,] _originalHeights;

	private void Start() {
		var data = terrain.terrainData;
		_originalHeights = data.GetHeights(0, 0, data.heightmapWidth, data.heightmapHeight);
	}

	private void OnDestroy() {
		terrain.terrainData.SetHeights(0, 0, _originalHeights);
	}
}
