using NUnit.Framework;

public class TileTests
{
    [Test]
    [TestCase(0, 5)]
    [TestCase(8, 0)]
    [TestCase(15, 6)]
    public void TestTileConstructor(int x, int y)
    {
        bool yIsZero = y == 0;
        Tile tile = new Tile(x, y);

        Assert.IsTrue(tile.X == x);
        Assert.IsTrue(tile.Y == y);
        Assert.IsTrue(tile.IsOccupied == false);
        Assert.IsTrue(tile.Building == null);
        Assert.IsTrue(tile.IsSupported == yIsZero);
    }

    [Test]
    [TestCase(false, false)]
    [TestCase(false, true)]
    [TestCase(true, true)]
    public void TestTileCanBuild(bool isOccupied, bool isSupported)
    {
        Tile tile = new Tile(1, 1)
        {
            IsOccupied = isOccupied,
            IsSupported = isSupported
        };

        bool expectedOutcome = !isOccupied && isSupported;
        Assert.IsTrue(tile.CanBuild(out string debugMessage) == expectedOutcome);

        string expectedMessage = "";
        if (isOccupied) expectedMessage = $"Tile_1_1 is occupied";
        if (!isSupported) expectedMessage = $"Tile_1_1 is not supported";
        Assert.IsTrue(debugMessage == expectedMessage);
    }
}
