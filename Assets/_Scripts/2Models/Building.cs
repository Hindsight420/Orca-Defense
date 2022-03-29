using EventCallbacks;

public class Building
{
    BuildingType buildingType;
    int x;
    int y;

    public BuildingType BuildingType { get => buildingType; private set => buildingType = value; }
    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }


    public Building(int x, int y, BuildingType buildingType)
    {
        X = x;
        Y = y;
        BuildingType = buildingType;

        new BuildingCreatedEvent().FireEvent(this);
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
