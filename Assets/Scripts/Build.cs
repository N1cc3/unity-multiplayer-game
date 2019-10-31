using Mirror;
using UnityEngine;
using static Game;
using static Mirror.NetworkServer;
using static UnityEngine.Quaternion;

public class Build : NetworkBehaviour {
	private Game _game;

	private void Awake() {
		_game = FindObjectOfType<Game>();
	}

	[Command]
	public void CmdBuild(Vector3 position, BuildingType buildingType) {
		var holo = Instantiate(_game.GetBuildingHolo(buildingType), position, identity);
		var holoScript = holo.GetComponent<BuildingHolo>();
		var success = false;

		if (holoScript.CanBeBuilt()) {
			var building = Instantiate(_game.GetBuilding(buildingType), position, identity);
			building.GetComponent<Owned>().Owner = connectionToClient.connectionId;
			_game.AddUnbuildable(building.GetComponentInChildren<Unbuildable>());
			Spawn(building);
			success = true;
		}

		Destroy(holo);
		TargetBuildingComplete(success);
	}

	[TargetRpc]
	private void TargetBuildingComplete(bool success) {
		Debug.Log(success);
	}
}
