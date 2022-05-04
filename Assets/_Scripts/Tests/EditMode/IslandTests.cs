using NUnit.Framework;
using OrcaDefense.Models;
using System;
using System.Linq;

public class IslandTests
{
    [Test]
    [TestCase(20, 10)]
    [TestCase(12, 69)]
    public void TestIslandConstructor(int x, int y)
    {
        Island island = new(x, y);

        Assert.AreEqual(island.Width, x);
        Assert.AreEqual(island.Height, y);
    }

    [Test]
    [TestCase(1, 1)]
    [TestCase(17, 6)]
    [TestCase(2, 8)]
    public void TestAdjacentTiles(int x, int y)
    {
        Island island = new(20, 10);

        var adjacentTiles = island.GetAdjacentTiles(x, y);
        var expectedTiles = new[] { new Tile(x + 1, y), new Tile(x - 1, y), new Tile(x, y + 1), new Tile(x, y - 1) };
        Assert.AreEqual(expectedTiles, adjacentTiles);
    }

    [Test]
    public void TestAdjacentTilesBorders()
    {
        Island island = new(20, 10);

        var adjacentTiles = island.GetAdjacentTiles(0, 0);
        var expectedTiles = new[] { new Tile(1, 0), new Tile(0, 1) };
        Assert.AreEqual(expectedTiles, adjacentTiles);

        adjacentTiles = island.GetAdjacentTiles(19, 5);
        expectedTiles = new[] { new Tile(18, 5), new Tile(19, 6), new Tile(19, 4) };
        Assert.AreEqual(expectedTiles, adjacentTiles);
    }
}
