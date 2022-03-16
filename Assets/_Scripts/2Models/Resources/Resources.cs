using EventCallbacks;
using System.Collections.Generic;
using UnityEngine;

public class Resources
{
    Dictionary<ResourceType, int> resourceDictionary = new();
    Resource wood;
    public Resource Wood { get => wood; set => wood = value; }

    Resource fish;
    public Resource Fish { get => fish; set => fish = value; }

    public Resources() : this(new Resource(ResourceType.Wood), new Resource(ResourceType.Fish))
    {

    }

    public Resources(Resource wood, Resource fish)
    {
        Wood = wood;
        Fish = fish;

        BuildingCreatedEvent.RegisterListener(OnBuildingCreated);
        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);
    }

    private void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        Wood.Amount -= buildingEvent.building.BuildingBase.Cost;
    }

    private void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Wood.Amount += buildingEvent.building.BuildingBase.Cost;
    }
}


public class Resource
{
    public static implicit operator int(Resource instance) { return instance.Amount; }

    //public static Resource operator +(Resource r) => r;
    //public static Resource operator +(Resource r, int i) => r + i;
    //public static Resource operator -(Resource r, int i) => r - i;

    readonly ResourceType type;
    public ResourceType Type => type;


    int amount;
    public int Amount
    {
        get => amount; 
        
        set
        {
            if (value < 0) // resources shouldn't go under 0
            {
                Debug.LogError($"{this} tried to set to {value}, setting amount to 0 instead");
                value = 0;
            }

            amount = value;
        }
    }

    public Resource(ResourceType type, int amount = 0)
    {
        this.type = type;
        Amount = amount;
    }

    public override string ToString()
    {
        return $"Resource ({Type}: {Amount})";
    }
}

public enum ResourceType
{
    Wood,
    Fish
}
