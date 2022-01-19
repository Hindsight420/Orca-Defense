using EventCallbacks;

public class Building
{
    int x;
    int y;
    private BuildingSettings buildingSettings;

    public int X { get => x; private set => x = value; }
    public int Y { get => y; private set => y = value; }
    public BuildingSettings BuildingSettings { get => buildingSettings; private set => buildingSettings = value; }


    public Building(int x, int y, BuildingSettings buildingSettings)
    {
        X = x;
        Y = y;
        BuildingSettings = buildingSettings;

        new BuildingCreatedEvent().FireEvent(this);
    }

    public void Remove()
    {
        new BuildingRemovedEvent().FireEvent(this);
    }

    public override string ToString()
    {
        return buildingSettings.name;
    }
}
