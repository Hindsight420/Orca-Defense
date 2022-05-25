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

    public Building(BuildingType buildingType, Tile tile)
    {
        BuildingType = buildingType;
        Tile = tile;
        tile.Building = this;
        X = tile.X;
        Y = tile.Y;
    }

    public override string ToString()
    {
        return $"Building ({buildingType}: {x}, {y})";
    }
}
