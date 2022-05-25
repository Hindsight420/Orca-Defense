using Assets._Scripts._3Managers;
using EventCallbacks;

public class Building
{
    BuildingType buildingType;
    int x;
    int y;

    public BuildingType BuildingType { get => buildingType; private set => buildingType = value; }
    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }

    int startTick;

    public Building(int x, int y, BuildingType buildingType)
    {
        X = x;
        Y = y;
        BuildingType = buildingType;

        new BuildingCreatedEvent().FireEvent(this);
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
