using NUnit.Framework;
using OrcaDefense.Models;

public class TileTests
{
    [Test]
    [TestCase(0, 5)]
    [TestCase(8, 0)]
    [TestCase(15, 6)]
    public void TestTileConstructor(int x, int y)
    {
        bool yIsZero = y == 0;
        Tile tile = new(x, y);

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
        Tile tile = new(1, 1)
        {
            IsOccupied = isOccupied,
            IsSupported = isSupported
        };

        bool expectedOutcome = !isOccupied && isSupported;
        Assert.AreEqual(tile.CanBuild(), expectedOutcome);
    }

    [Test]
    public void TestTileComparison()
    {
        Tile t1 = new(1, 1);
        Tile t2 = new(1, 1);
        Tile t3 = new(1, 2);
        Assert.IsTrue(t1 == t2);
        Assert.IsTrue(t1 != t3);
    }
}
