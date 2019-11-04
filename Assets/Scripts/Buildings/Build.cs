using Mirror;
using UnityEngine;
using static GameController;
using static Mirror.NetworkServer;
using static UnityEngine.Quaternion;

public class Build : NetworkBehaviour {
	private GameController _game;

	private void Awake() {
		_game = FindObjectOfType<GameController>();
	}

	[Command]
	public void CmdBuild(Vector3 position, BuildingType buildingType) {
		var buildingHolo = _game.GetBuildingHolo(buildingType);
		var holo = Instantiate(buildingHolo, position, identity);
		var holoScript = holo.GetComponent<BuildingHolo>();
		var success = false;

		if (holoScript.CanBeBuilt()) {
			var building = Instantiate(_game.GetBuilding(buildingType), position, identity);
			building.GetComponent<Owned>().owner = connectionToClient.connectionId;
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
