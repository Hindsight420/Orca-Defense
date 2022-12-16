using Assets._Scripts._3Managers;
using EventCallbacks;
using OrcaDefense.Models;
using System.Collections.Generic;
using System.Linq;

public class Building
{
    BuildingType type;
    Tile tile;
    int x;
    int y;
    BuildingState state;
    ResourceList constructionResources = new();
    ResourceList remainingResources = new();

    public BuildingType Type { get => type; private set => type = value; }
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
    public ResourceList ConstructionResources
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
    public ResourceList RemainingResources { get => remainingResources; set => remainingResources = value; }

    private Logger Logger { get => Logger.Instance; }

    int startTick;

    public Building(BuildingType buildingType, Tile tile, BuildingState state = BuildingState.Planned)
    {
        Type = buildingType;
        Tile = tile;
        tile.Building = this;
        X = tile.X;
        Y = tile.Y;
        this.state = state;

        foreach (ResourceValue resourceValue in buildingType.Cost.ResourceValueList)
        {
            ConstructionResources.Add(new(resourceValue.Type));
            RemainingResources.Add(new(resourceValue.Type, resourceValue.Amount));
        }
    }

    public void AddResources(ResourceList resources)
    {
        resources.TransferTo(ConstructionResources);
        RemainingResources = Type.Cost.Minus(ConstructionResources);

        //foreach (ResourceValue resource in resources)
        //{
        //    ResourceType type = resource.Type;

        //    // Maybe add a try catch block for these 2 lines? In case we add an invalid resource.
        //    //ResourceValue currentResourceValue = ConstructionResources.First(r => r.Type == type);
        //    //ResourceValue requiredResourceValue = Type.Cost.First(r => r.Type == type);

        //    //resource.TransferTo(currentResourceValue);
        //    if (currentResourceValue > requiredResourceValue) // Not a safety precaution, but we need to know if this ever happens
        //        Logger.LogMessage($"{this} received too many resources: {currentResourceValue - requiredResourceValue}.", Logger.LogType.Error);

        //    //ResourceValue remainingResource = RemainingResources.First(r => r.Type == type);
        //    //remainingResource.Amount = requiredResourceValue - currentResourceValue;
        //    //if (remainingResource.Amount == 0) RemainingResources.Remove(remainingResource);
        //}
    }

    public void Construct()
    {
        if (State != BuildingState.Planned)
        {
            Logger.LogMessage($"{this} is in '{State}' state, but it's trying to construct.", Logger.LogType.Error);
            return;
        }

        if (!ConstructionResources.Equals(Type.Cost))
        {
            Logger.LogMessage($"{this} can't be constructed because of a mismatch in construction resources. Current: {ConstructionResources}. Expected: {Type.Cost}.", Logger.LogType.Error);
            return;
        }
        
        // Everything is in order, time to construct
        State = BuildingState.Constructed;

        if (type.TicksPerIncome is not null && type.Income is not null)
        {
            TimeTicker.OnTick += OnTick;
            startTick = TimeTicker.CurrentTick;
        }
    }

    private void OnTick(object obj, int tick)
    {
        if (TimeTicker.GetInnerTick(startTick) % Type.TicksPerIncome == 0)
        {
            if (Type.Income != null)
            {
                new IncomeEvent().FireEvent(Type.Income);
            }
        }
    }

    public void Remove()
    {
        new BuildingRemovedEvent().FireEvent(this);
    }

    public override string ToString()
    {
        return $"Building ({type}: {x}, {y})";
    }
}

public enum BuildingState
{
    Preview, // Redundant?
    Planned,
    Constructed
}
