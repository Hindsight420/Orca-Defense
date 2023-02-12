using Assets._Scripts._3Managers;
using EventCallbacks;
using OrcaDefense.Models;

public class Building
{
    private BuildingState _state;
    private ResourceList _constructionResources = new();
    private int startTick;
    private readonly Logger _logger = Logger.Instance;

    public BuildingType Type { get; }
    public Tile Tile { get; }
    public int X { get; }
    public int Y { get; }
    public BuildingState State
    {
        get => _state; set
        {
            _state = value;
            new BuildingChangedEvent().FireEvent(this);
        }
    }
    public ResourceList ConstructionResources
    {
        get => _constructionResources;
        private set
        {
            if (State != BuildingState.Planned)
            {
                _logger.LogMessage($"{this} is in '{State}' state, but the construction resources are trying to alter.", Logger.LogType.Error);
                return;
            }
            _constructionResources = value;
        }
    }
    public ResourceList RemainingResources { get; set; } = new();

    public Building(BuildingType buildingType, Tile tile, BuildingState state = BuildingState.Planned)
    {
        Type = buildingType;
        Tile = tile;
        tile.Building = this;
        X = tile.X;
        Y = tile.Y;
        _state = state;

        RemainingResources = buildingType.Cost.Copy();
    }

    public void AddResources(ResourceList resources)
    {
        resources.TransferTo(ConstructionResources);
        RemainingResources = Type.Cost.Minus(ConstructionResources);
    }

    public void Construct()
    {
        if (State != BuildingState.Planned)
        {
            _logger.LogMessage($"{this} is in '{State}' state, but it's trying to construct.", Logger.LogType.Error);
            return;
        }

        if (!ConstructionResources.Equals(Type.Cost))
        {
            _logger.LogMessage($"{this} can't be constructed because of a mismatch in construction resources. Current: {ConstructionResources}. Expected: {Type.Cost}.", Logger.LogType.Error);
            return;
        }
        
        // Everything is in order, time to construct
        State = BuildingState.Constructed;

        if (Type.TicksPerIncome != 0 && Type.Income is not null)
        {
            TimeTicker.OnTick += OnTick;
            startTick = TimeTicker.CurrentTick;
        }
    }

    private void OnTick(object obj, int tick)
    {
        if (TimeTicker.GetInnerTick(startTick) % Type.TicksPerIncome == 0)
        {
            if (Type.Income is not null) // double check, irrelevant?
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
        return $"Building ({Type}: {X}, {Y})";
    }
}

public enum BuildingState
{
    Preview, // Redundant?
    Planned,
    Constructed
}
