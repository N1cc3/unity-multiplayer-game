using System.Linq;
using Mirror;
using UnityEngine;
using static GameController;
using static Mirror.NetworkServer;

public class Spawn : NetworkBehaviour {
	private GameController _game;

	private void Awake() {
		_game = FindObjectOfType<GameController>();
	}

	[Command]
	public void CmdSpawn(VehicleType vehicleType) {
		var respawns = GameObject.FindGameObjectsWithTag("Respawn");
		var ownedRespawn = respawns.First(r => r.GetComponentInParent<Owned>().owner == connectionToClient.connectionId);
		var vehicle = Instantiate(_game.GetVehicle(vehicleType), ownedRespawn.transform.position,
			ownedRespawn.transform.rotation);
		var success = false;

		vehicle.GetComponent<Owned>().owner = connectionToClient.connectionId;
		Spawn(vehicle);
		success = true;

		TargetBuildingComplete(success);
	}

	[TargetRpc]
	private void TargetBuildingComplete(bool success) {
		Debug.Log(success);
	}
}
