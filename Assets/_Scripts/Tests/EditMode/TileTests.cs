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
        Tile tile = new(x, y);

        Assert.AreEqual(tile.X, x);
        Assert.AreEqual(tile.Y, y);
        Assert.AreEqual(tile.Building, null);
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
