namespace OrcaDefense.Models
{
    public class Tile
    {
        bool isOccupied;
        bool isSupported;

        Building building;

        public readonly int X;
        public readonly int Y;
        public IBuildingValidator Validator { get; set; }

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

            Validator = new BaseBuildingValidator();
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

        public override bool Equals(object other)
        {
            Tile t = other as Tile;
            return X == t.X && Y == t.Y;
        }

        public static bool operator ==(Tile t1, Tile t2)
        {
            return t1.X == t2.X && t1.Y == t2.Y;
        }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1.X == t2.X && t1.Y == t2.Y);
        }
    }
}
