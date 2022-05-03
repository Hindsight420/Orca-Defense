using EventCallbacks;
using OrcaDefense.Models;
using System.Collections.Generic;
using UnityEngine;

public class IslandView : MonoBehaviour
{
    Dictionary<Building, GameObject> buildingGameObjectMap;
    Dictionary<int, GameObject> roofGameObjectMap;

    public Island Island;
    public GameObject RoofPrefab;

    void Awake()
    {
        buildingGameObjectMap = new();
        roofGameObjectMap = new();
    }

    public void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        buildingGameObjectMap.Remove(b, out GameObject building_go);
        Destroy(building_go);

        UpdateRoof(buildingEvent.Building.X);
    }

    public void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        GameObject building_go = UpdateBuildingGameObject(buildingEvent);

        buildingGameObjectMap.Add(buildingEvent.Building, building_go);

        UpdateRoof(buildingEvent.Building.X);
    }

    void UpdateRoof(int x)
    {
        Tile t = Island.GetHighestFreeTileAt(x);

        roofGameObjectMap.TryGetValue(x, out GameObject roofGO);

        // Destroy the roof if there's no buildings
        if (t.Y == 0)
        {
            roofGameObjectMap[x] = null;
            Destroy(roofGO);
        }

        roofGO = roofGO != null ? roofGO : Instantiate(RoofPrefab, transform);
        roofGO.transform.position = new Vector3(t.X, t.Y);

        roofGameObjectMap[x] = roofGO;
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
