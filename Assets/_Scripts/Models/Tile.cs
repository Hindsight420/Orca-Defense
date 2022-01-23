public class Tile
{
    bool isOccupied;
    bool isSupported;

    Building building;

    public readonly int X;
    public readonly int Y;

    // TODO: clean up this mess
    public bool IsSupported { get => isSupported; set => isSupported = value; }

    public Building Building
    {
        get => building;
        set
        {
            isOccupied = true;
            building = value;
        }
    }

    public Tile(int x, int y)
    {
        X = x;
        Y = y;

        if (y == 0) IsSupported = true;
    }

    public bool CanBuild(out string debugMessage)
    {
        bool canBuild = true;
        string message = $"Tile_{X}_{Y} is ";
        if (isOccupied)
        {
            canBuild = false;
            message += $"occupied by {Building.BuildingBase.name}";
        }

        if (!IsSupported)
        {
            canBuild = false;
            message += $"not supported";
        }

        debugMessage = message;
        return canBuild;
    }
}
