using System.Linq;
using Mirror;
using UnityEngine;
using static Mirror.NetworkServer;
using static UnityEngine.Quaternion;

public class Spawn : NetworkBehaviour {
	private GameObject _mediumTransportPrefab;

	private void Awake() {
		_mediumTransportPrefab = Resources.Load("medium_transport") as GameObject;
	}

	[Command]
	public void CmdSpawn() {
		var respawns = GameObject.FindGameObjectsWithTag("Respawn");
		var ownedRespawn = respawns.First(r => r.GetComponentInParent<Owned>().owner == connectionToClient.connectionId);
		var spawnPosition = ownedRespawn.transform.position;
		var vehicle = Instantiate(_mediumTransportPrefab, spawnPosition, identity);
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
