using EventCallbacks;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField]
    private ResourceView _view;
    private readonly ResourceList _resources = new();
    private readonly ResourceList _resourcesInHolding = new();

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
        _view.InitializeCounters(_resources);
    }

    void FillResourceList()
    {
        foreach (ResourceType type in DataSystem.Instance.ResourceTypes)
        {
            _resources.Add(new Resource(type, 500)); // TODO: Clean up temporary, hardcoded starting value
            _resourcesInHolding.Add(new Resource(type));
        }
    }

    void ExpendResources(ResourceList resources)
    {
        this._resources.TransferTo(_resourcesInHolding, resources);

        new ResourcesChangedEvent().FireEvent(this._resources);
    }

    void AddResources(ResourceList resources)
    {
        this._resources.Add(resources);

        new ResourcesChangedEvent().FireEvent(this._resources);
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
        return this._resources.CheckResourcesAvailability(resources);
    }
}
