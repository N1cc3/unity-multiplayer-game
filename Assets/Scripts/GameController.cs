using System.Collections.Generic;
using UnityEngine;
using static GameController.BuildingType;
using static GameController.VehicleType;

public class GameController : MonoBehaviour {
	private void Awake() {
		InitUnbuildables();
		InitBuildings();
		InitVehicles();
	}

	#region Unbuildables

	private readonly HashSet<Unbuildable> _unbuildables = new HashSet<Unbuildable>();

	private void InitUnbuildables() {
		foreach (var unbuildable in FindObjectsOfType<Unbuildable>()) _unbuildables.Add(unbuildable);
	}

	public void AddUnbuildable(Unbuildable unbuildable) {
		_unbuildables.Add(unbuildable);
	}

	public void RemoveUnbuildable(Unbuildable unbuildable) {
		_unbuildables.Remove(unbuildable);
	}

	public IEnumerable<Unbuildable> Unbuildables() {
		return _unbuildables;
	}

	#endregion

	#region Buildings

	private readonly Dictionary<BuildingType, GameObject> _buildings = new Dictionary<BuildingType, GameObject>();
	private readonly Dictionary<BuildingType, GameObject> _buildingHolos = new Dictionary<BuildingType, GameObject>();

	private void InitBuildings() {
		void AddBuilding(BuildingType type, string buildingName) {
			_buildings.Add(type, Resources.Load(buildingName) as GameObject);
			_buildingHolos.Add(type, Resources.Load(buildingName + "_holo") as GameObject);
		}

		AddBuilding(Headquarters, "headquarters");
		AddBuilding(HmgTurret, "hmg_turret");
		AddBuilding(Factory1, "factory_1");
	}

	public enum BuildingType {
		Headquarters,
		HmgTurret,
		Factory1,
	}

	public GameObject GetBuilding(BuildingType buildingType) {
		return _buildings[buildingType];
	}

	public GameObject GetBuildingHolo(BuildingType buildingType) {
		return _buildingHolos[buildingType];
	}

	#endregion

	#region Vehicles

	private readonly Dictionary<VehicleType, GameObject> _vehicles = new Dictionary<VehicleType, GameObject>();

	private void InitVehicles() {
		void AddBuilding(VehicleType type, string vehicleName) {
			_vehicles.Add(type, Resources.Load(vehicleName) as GameObject);
		}

		AddBuilding(MediumTransport, "medium_transport");
		AddBuilding(Tank, "tank");
		AddBuilding(Buggy, "scavbuggy");
	}

	public enum VehicleType {
		MediumTransport,
		Tank,
		Buggy,
	}

	public GameObject GetVehicle(VehicleType vehicleType) {
		return _vehicles[vehicleType];
	}

	#endregion
}
