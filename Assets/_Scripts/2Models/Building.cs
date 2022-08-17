using Assets._Scripts._3Managers;
using EventCallbacks;
using OrcaDefense.Models;

public class Building
{
    BuildingType buildingType;
    Tile tile;
    int x;
    int y;

    public BuildingType BuildingType { get => buildingType; private set => buildingType = value; }
    public Tile Tile { get => tile; private set => tile = value; }
    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }

    int startTick;

    public Building(BuildingType buildingType, Tile tile)
    {
        BuildingType = buildingType;
        Tile = tile;
        tile.Building = this;
        X = tile.X;
        Y = tile.Y;

        if (buildingType.TicksPerIncome is not null && buildingType.Income is not null )
        {
            TimeTicker.OnTick += OnTick;
            startTick = TimeTicker.CurrentTick;
        }
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
