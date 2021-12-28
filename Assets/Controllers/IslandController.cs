using EventCallbacks;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    Dictionary<Building, GameObject> buildingGameObjectMap;
    public Sprite squareSprite;

    public static IslandController Instance { get; private set; }

    Island island;
    public Island Island { get => island; private set => island = value; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

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

    void OnBuildingCreated(BuildingEvent buildingEvent)
    {
        Building building = buildingEvent.building;
        GameObject building_go = new();

        buildingGameObjectMap.Add(building, building_go);

        building_go.name = $"{building.Type}_{building.X}_{building.Y}";
        building_go.transform.position = new Vector3(building.X, building.Y, 0);
        building_go.transform.SetParent(this.transform);

        SpriteRenderer sr = building_go.AddComponent<SpriteRenderer>();
        sr.sprite = squareSprite;
    }
}
