using EventCallbacks;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : Singleton<IslandController>
{
    Dictionary<Building, GameObject> buildingGameObjectMap;

    Island island;
    public Island Island { get => island; private set => island = value; }

    // Start is called before the first frame update
    void Start()
    {
        buildingGameObjectMap = new Dictionary<Building, GameObject>();

        Island = new Island();
        BuildingCreatedEvent.RegisterListener(OnBuildingCreated);

        // Instantiate any buildings that already exist (from loading an existing save)
        foreach (Building building in Island.buildings)
        {
            new BuildingCreatedEvent().FireEvent(building);
        }
    }

    void Update()
    {

    }

    void OnBuildingChanged(BuildingEvent buildingEvent)
    {
        GameObject building_go = UpdateBuildingGameObject(buildingEvent);
    }

    void OnBuildingCreated(BuildingEvent buildingEvent)
    {
        GameObject building_go = UpdateBuildingGameObject(buildingEvent);

        buildingGameObjectMap.Add(buildingEvent.building, building_go);
    }

    GameObject UpdateBuildingGameObject(BuildingEvent buildingEvent)
    {
        Building building = buildingEvent.building;
        GameObject building_go = new();

        building_go.name = $"{building.BuildingBase.name}_{building.X}_{building.Y}";
        building_go.transform.position = new Vector3(building.X, building.Y, 0);
        building_go.transform.SetParent(transform);

        SpriteRenderer sr = building_go.AddComponent<SpriteRenderer>();
        sr.sprite = building.BuildingBase.Sprite;

        return building_go;
    }
}
