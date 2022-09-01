using Assets._Scripts._3Managers;
using EventCallbacks;
using OrcaDefense.Models;
using System.Collections.Generic;
using System.Linq;

public class Building
{
    BuildingType buildingType;
    Tile tile;
    int x;
    int y;
    BuildingState state;
    List<ResourceValue> constructionResources = new();

    public BuildingType BuildingType { get => buildingType; private set => buildingType = value; }
    public Tile Tile { get => tile; private set => tile = value; }
    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }
    public BuildingState State
    {
        get => state; set
        {
            state = value;
            new BuildingChangedEvent().FireEvent(this);
        }
    }
    public List<ResourceValue> ConstructionResources
    {
        get => constructionResources;
        private set
        {
            if (State != BuildingState.Planned)
            {
                Logger.LogMessage($"{this} is in '{State}' state, but the construction resources are trying to alter.", Logger.LogType.Error);
                return;
            }
            constructionResources = value;
        }
    }

    private Logger Logger { get => Logger.Instance; }
    readonly int startTick;

    public Building(BuildingType buildingType, Tile tile, BuildingState state = BuildingState.Planned)
    {
        BuildingType = buildingType;
        Tile = tile;
        tile.Building = this;
        X = tile.X;
        Y = tile.Y;
        State = state;
        ConstructionResources.AddRange(buildingType.Cost.Select(r => { r.Amount = 0; return r; }));

        if (buildingType.TicksPerIncome is not null && buildingType.Income is not null)
        {
            TimeTicker.OnTick += OnTick;
            startTick = TimeTicker.CurrentTick;
        }
    }

    public void AddResources(List<ResourceValue> resources)
    {
        foreach (ResourceValue resource in resources)
        {
            ResourceType type = resource.Type;

            // Maybe add a try catch block for these 2 lines? In case we add an invalid resource.
            ResourceValue currentResourceValue = ConstructionResources.First(r => r.Type == type);
            ResourceValue requiredResourceValue = BuildingType.Cost.First(r => r.Type == type);

            resource.TransferTo(currentResourceValue);
            if (currentResourceValue > requiredResourceValue) // Not a safety precaution, but we need to know if this ever happens
                Logger.LogMessage($"{this} received too many resources: {currentResourceValue - requiredResourceValue}.", Logger.LogType.Error);
        }
    }

    public void Construct()
    {
        if (State != BuildingState.Planned)
        {
            Logger.LogMessage($"{this} is in '{State}' state, but it's trying to construct.", Logger.LogType.Error);
            return;
        }

        foreach (ResourceValue resource in BuildingType.Cost)
        {
            ResourceType type = resource.Type;
            ResourceValue currentResourceValue = ConstructionResources.First(r => r.Type == type);
            if (currentResourceValue != resource)
            {
                Logger.LogMessage($"{this} can't be constructed because of a mismatch in construction resources. Current: {currentResourceValue}. Expected: {resource}.", Logger.LogType.Error);
                return;
            }
        }

        State = BuildingState.Constructed;
    }

    private void OnTick(object obj, int tick)
    {
        if (TimeTicker.GetInnerTick(startTick) % BuildingType.TicksPerIncome == 0)
        {
            if (BuildingType.Income != null)
            {
                new IncomeEvent().FireEvent(BuildingType.Income);
            }
        }
    }

    public void Remove()
    {
        new BuildingRemovedEvent().FireEvent(this);
    }

    public override string ToString()
    {
        return $"Building ({buildingType}: {x}, {y})";
    }
}

public enum BuildingState
{
    Preview, // Redundant?
    Planned,
    Constructed
}
