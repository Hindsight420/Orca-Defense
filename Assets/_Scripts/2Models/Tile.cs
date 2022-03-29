public class Tile
{
    bool isOccupied;
    bool isSupported;

    Building building;

    public readonly int X;
    public readonly int Y;

    // TODO: clean up this mess

    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public bool IsSupported { get => isSupported; set => isSupported = value; }

    public Building Building
    {
        get => building;
        set
        {
            building = value;
            isOccupied = (building != null);
        }
    }

    public Tile(int x, int y)
    {
        X = x;
        Y = y;

        if (y == 0) IsSupported = true;
    }

    public bool CanBuild()
    {
        if (IsOccupied)
            return false;
        if (!IsSupported)
            return false;
        return true;
    }

    public override string ToString()
    {
        return $"Tile: {X}, {Y}";
    }
}
