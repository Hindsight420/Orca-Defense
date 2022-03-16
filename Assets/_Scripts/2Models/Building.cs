using EventCallbacks;

public class Building
{
    int x;
    int y;
    BuildingBase buildingBase;

    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }
    public BuildingBase BuildingBase { get => buildingBase; private set => buildingBase = value; }


    public Building(int x, int y, BuildingBase buildingBase)
    {
        X = x;
        Y = y;
        BuildingBase = buildingBase;

        new BuildingCreatedEvent().FireEvent(this);
    }

    public void Remove()
    {
        new BuildingRemovedEvent().FireEvent(this);
    }

    public override string ToString()
    {
        return buildingBase.name;
    }
}
