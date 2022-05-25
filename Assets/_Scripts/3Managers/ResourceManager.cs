using EventCallbacks;
using System.Collections.Generic;
using System.Linq;

public class ResourceManager : Singleton<ResourceManager>
{
    public ResourceView View;

    readonly List<ResourceValue> resources = new();

    protected override void Awake()
    {
        base.Awake();

        BuildingCreatedEvent.RegisterListener(OnBuildingCreated);
        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);
        IncomeEvent.RegisterListener(OnIncome);
    }

    void Start()
    {
        FillResourceList();
        View.InitializeCounters(resources);
    }

    void FillResourceList()
    {
        foreach (ResourceType type in DataSystem.Instance.ResourceTypes)
        {
            resources.Add(new ResourceValue(type, 500)); // TODO: Clean up temporary, hardcoded starting value
        }
    }

    ResourceValue GetResourceValue(ResourceType type)
    {
        return resources.First(r => r.Type == type);
    }

    void ExpendResources(List<ResourceValue> resourceValues)
    {
        foreach (ResourceValue resourceValue in resourceValues)
            ExpendResources(resourceValue);
    }

    void ExpendResources(ResourceValue resourceValue)
    {
        ResourceValue resource = GetResourceValue(resourceValue.Type);
        resource.Amount -= resourceValue;

        new ResourceValueChangedEvent().FireEvent(resource);
    }

    void AddResources(List<ResourceValue> resourceValues)
    {
        foreach (ResourceValue resourceValue in resourceValues)
            AddResources(resourceValue);
    }

    void AddResources(ResourceValue resourceValue)
    {
        ResourceValue resource = resources.First(r => r.Type == resourceValue.Type);
        resource.Amount += resourceValue;

        new ResourceValueChangedEvent().FireEvent(resource);
    }

    void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        ExpendResources(buildingEvent.Building.BuildingType.Cost);
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        AddResources(buildingEvent.Building.BuildingType.Cost);
    }

    void OnIncome(IncomeEvent incomeEvent)
    {
        incomeEvent.ResourceValues.ForEach(resource => AddResources(resource));
    }

    public bool CheckResourcesAvailability(List<ResourceValue> resourceValues)
    {
        foreach (ResourceValue resourceValue in resourceValues)
            if(!CheckResourcesAvailability(resourceValue)) return false;
        return true;
    }

    public bool CheckResourcesAvailability(ResourceValue resourceValue)
    {
        return GetResourceValue(resourceValue.Type) >= resourceValue;
    }
}
