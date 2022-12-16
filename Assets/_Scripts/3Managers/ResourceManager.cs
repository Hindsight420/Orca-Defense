using EventCallbacks;
using System.Collections.Generic;
using System.Linq;

public class ResourceManager : Singleton<ResourceManager>
{
    public ResourceView View;

    readonly ResourceList resources = new();
    readonly ResourceList resourcesInHolding = new();

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
            resources.Add(new Resource(type, 500)); // TODO: Clean up temporary, hardcoded starting value
            resourcesInHolding.Add(new Resource(type));
        }
    }

    void ExpendResources(ResourceList resources)
    {
        this.resources.TransferTo(resourcesInHolding, resources);

        new ResourcesChangedEvent().FireEvent(this.resources);
    }

    void AddResources(ResourceList resources)
    {
        resources.TransferTo(this.resources);

        new ResourcesChangedEvent().FireEvent(this.resources);
    }

    void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        ExpendResources(buildingEvent.Building.Type.Cost);
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        AddResources(buildingEvent.Building.Type.Cost);
    }

    void OnIncome(IncomeEvent incomeEvent)
    {
        AddResources(incomeEvent.Resources);
    }

    public bool CheckResourcesAvailability(ResourceList resources)
    {
        return this.resources.CheckResourcesAvailability(resources);
    }
}
