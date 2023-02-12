using EventCallbacks;
using OrcaDefense.Models;
using System.Collections.Generic;
using UnityEngine;

public class IslandView : MonoBehaviour
{
    private Dictionary<Building, GameObject> _buildingGameObjectMap;
    private Dictionary<Tile, GameObject> _roofGameObjectMap;
    private readonly GameObject _roofPrefab;

    public Island Island { get; set; }
    void Awake()
    {
        _buildingGameObjectMap = new();
        _roofGameObjectMap = new();
    }

    public void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        GameObject building_go = UpdateBuildingGameObject(b);
        _buildingGameObjectMap.Add(buildingEvent.Building, building_go);

        UpdateRoofs(b);
    }

    public void OnBuildingChanged(BuildingChangedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        UpdateBuildingGameObject(b);

        UpdateRoofs(b);
    }

    public void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        _buildingGameObjectMap.Remove(b, out GameObject building_go);
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
        _roofGameObjectMap.TryGetValue(t, out GameObject roofGO);

        // Should we render a roof?
        if (t.Validator.ShouldRenderRoof(Island, b.Type) == false)
        {
            // No roof to remove?
            if (roofGO is null) return;

            // Remove roof
            _roofGameObjectMap[t] = null;
            Destroy(roofGO);
            return;
        }

        // Already a roof?
        if (roofGO is not null) return;

        // Create roof
        roofGO = roofGO != null ? roofGO : Instantiate(RoofPrefab, transform);
        roofGO.transform.position = new Vector3(t.X, t.Y + 1);

        _roofGameObjectMap[t] = roofGO;
    }

    GameObject UpdateBuildingGameObject(Building b)
    {
        if (!_buildingGameObjectMap.TryGetValue(b, out GameObject building_go)) 
            building_go = Instantiate(b.Type.Prefab);

        building_go.name = $"{b.Type.name}_{b.X}_{b.Y}";
        building_go.transform.position = new Vector3(b.X, b.Y, 0);
        building_go.transform.SetParent(transform);
        SpriteRenderer sr = building_go.GetComponent<SpriteRenderer>();
        sr.color = b.State == BuildingState.Planned ? new Color(1f, 1f, 1f, .5f) : new Color(1f, 1f, 1f, 0f);

        return building_go;
    }
}
