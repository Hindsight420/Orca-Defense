using EventCallbacks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : Singleton<IslandController>
{
    Dictionary<Building, GameObject> buildingGameObjectMap;
    Dictionary<int, GameObject> roofGameObjectMap;

    Island island;
    public Island Island { get => island; private set => island = value; }

    public GameObject RoofPrefab;

    public int Width;
    public int Height;

    void Start()
    {
        buildingGameObjectMap = new Dictionary<Building, GameObject>();
        roofGameObjectMap = new Dictionary<int, GameObject>();

        Island = new Island(Width, Height);
        BuildingCreatedEvent.RegisterListener(OnBuildingCreated);
        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);

        // Instantiate any buildings that already exist (from loading an existing save)
        foreach (Building building in Island.buildings)
        {
            new BuildingCreatedEvent().FireEvent(building);
        }
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Building b = buildingEvent.building;
        buildingGameObjectMap.Remove(b, out GameObject building_go);
        Destroy(building_go);

        UpdateRoof(buildingEvent.building.X);
    }

    void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        GameObject building_go = UpdateBuildingGameObject(buildingEvent);

        buildingGameObjectMap.Add(buildingEvent.building, building_go);

        UpdateRoof(buildingEvent.building.X);
    }

    void UpdateRoof(int x)
    {
        Tile t = island.GetHighestFreeTileAt(x);

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
        Building building = buildingEvent.building;
        GameObject building_go = Instantiate(building.BuildingBase.Prefab);

        building_go.name = $"{building.BuildingBase.name}_{building.X}_{building.Y}";
        building_go.transform.position = new Vector3(building.X, building.Y, 0);
        building_go.transform.SetParent(transform);

        return building_go;
    }
}
