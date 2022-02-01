using EventCallbacks;
using System;
using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    int dogecoin;
    public int Dogecoin { get => dogecoin; private set => dogecoin = value; }

    float time = 0.0f;
    public float gainzz;

    public GameObject Crypto;

    public static ResourceController Instance { get; private set; }

    void Start()
    {
        Instance = this;

        BuildingCreatedEvent.RegisterListener(OnBuildingCreated);
        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= gainzz)
        {
            time = 0.0f;
            dogecoin++;
        }

        Crypto.GetComponent<TextMeshProUGUI>().text = $"{dogecoin} Ð";
    }

    public bool DoIHaveEnough(int cost)
    {
        return dogecoin > cost;
    }

    private void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        dogecoin -= buildingEvent.building.BuildingBase.Cost;
    }

    private void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        dogecoin += buildingEvent.building.BuildingBase.Cost;
    }
}
