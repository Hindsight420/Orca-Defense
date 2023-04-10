namespace OrcaDefense.Models
{
    public class Tile
    {
        public int Y { get; }
        public int X { get; }
        public Building Building { get; set; }
        public IBuildingValidator Validator { get; set; }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            Validator = new BaseBuildingValidator(this);
        }

        public override string ToString()
        {
            return $"Tile: {X}, {Y}";
        }

        public override bool Equals(object other)
        {
            if (other is null) return false;
            Tile t = other as Tile;
            return X == t.X && Y == t.Y;
        }

        public static bool operator ==(Tile t1, Tile t2)
        {
            if (t1 is null) return t2 is null;
            return t1.Equals(t2);
        }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1 == t2);
        }

        public override int GetHashCode()
        {
            return int.Parse(X.ToString() + Y.ToString());
        }
    }
}
