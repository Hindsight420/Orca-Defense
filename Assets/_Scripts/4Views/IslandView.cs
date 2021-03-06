using EventCallbacks;
using OrcaDefense.Models;
using System.Collections.Generic;
using UnityEngine;

public class IslandView : MonoBehaviour
{
    Dictionary<Building, GameObject> buildingGameObjectMap;
    Dictionary<Tile, GameObject> roofGameObjectMap;

    public Island Island;
    public GameObject RoofPrefab;

    void Awake()
    {
        buildingGameObjectMap = new();
        roofGameObjectMap = new();
    }

    public void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        GameObject building_go = UpdateBuildingGameObject(buildingEvent);
        buildingGameObjectMap.Add(buildingEvent.Building, building_go);

        UpdateRoofs(b);
    }

    public void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        buildingGameObjectMap.Remove(b, out GameObject building_go);
        Destroy(building_go);

        UpdateRoofs(b);
    }

    void UpdateRoofs(Building b)
    {
        // Update the roof on this building
        UpdateRoof(b);

        // Update the roof on the building below
        Building buildingBelow = Island.Down(b.X, b.Y)?.Building;
        if (buildingBelow is not null) UpdateRoof(buildingBelow);
    }

    void UpdateRoof(Building b)
    {
        if (b is null) return;

        Tile t = b.Tile;
        roofGameObjectMap.TryGetValue(t, out GameObject roofGO);

        // Should we render a roof?
        if (t.Validator.ShouldRenderRoof(Island, b.BuildingType) == false)
        {
            // No roof to remove?
            if (roofGO is null) return;

            // Remove roof
            roofGameObjectMap[t] = null;
            Destroy(roofGO);
            return;
        }

        // Already a roof?
        if (roofGO is not null) return;

        // Create roof
        roofGO = roofGO != null ? roofGO : Instantiate(RoofPrefab, transform);
        roofGO.transform.position = new Vector3(t.X, t.Y + 1);

        roofGameObjectMap[t] = roofGO;
    }

    GameObject UpdateBuildingGameObject(BuildingCreatedEvent buildingEvent)
    {
        Building building = buildingEvent.Building;
        GameObject building_go = Instantiate(building.BuildingType.Prefab);

        building_go.name = $"{building.BuildingType.name}_{building.X}_{building.Y}";
        building_go.transform.position = new Vector3(building.X, building.Y, 0);
        building_go.transform.SetParent(transform);

        return building_go;
    }
}
