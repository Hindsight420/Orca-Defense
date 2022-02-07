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

        Assert.AreEqual(tile.X, x);
        Assert.AreEqual(tile.Y, y);
        Assert.AreEqual(tile.IsOccupied, false);
        Assert.AreEqual(tile.Building, null);
        Assert.AreEqual(tile.IsSupported, yIsZero);
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
        Assert.AreEqual(tile.CanBuild(out string debugMessage), expectedOutcome);

        string expectedMessage = "";
        if (isOccupied) expectedMessage = $"Tile_1_1 is occupied";
        if (!isSupported) expectedMessage = $"Tile_1_1 is not supported";
        Assert.AreEqual(debugMessage, expectedMessage);
    }
}
