using System;
using System.Collections.Generic;
using UnityEngine;
using static Game.BuildingType;

public class Game : MonoBehaviour {
	private void Start() {
		InitUnbuildables();
		InitBuildings();
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
	}

	public enum BuildingType {
		Headquarters,
		HmgTurret
	}

	public GameObject GetBuilding(BuildingType buildingType) {
		return _buildings[buildingType];
	}

	public GameObject GetBuildingHolo(BuildingType buildingType) {
		return _buildingHolos[buildingType];
	}

	#endregion
}
