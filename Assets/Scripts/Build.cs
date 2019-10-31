using Mirror;
using UnityEngine;
using static Mirror.NetworkServer;
using static UnityEngine.Quaternion;

public class Build : NetworkBehaviour {
	private Game _game;
	private GameObject _headquartersPrefab;
	private GameObject _headquartersHoloPrefab;

	private void Awake() {
		_headquartersPrefab = Resources.Load("headquarters") as GameObject;
		_headquartersHoloPrefab = Resources.Load("headquarters_holo") as GameObject;
		_game = FindObjectOfType<Game>();
	}

	[Command]
	public void CmdBuild(Vector3 position) {
		var holo = Instantiate(_headquartersHoloPrefab, position, identity);
		var holoScript = holo.GetComponent<BuildingHolo>();
		var success = false;

		if (holoScript.CanBeBuilt()) {
			var building = Instantiate(_headquartersPrefab, position, identity);
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
