using NUnit.Framework;

public class IslandTests
{
    [Test]
    [TestCase(20, 10)]
    [TestCase(12, 69)]
    public void TestIslandConstructor(int x, int y)
    {
        Island island = new Island(x, y);

        Assert.AreEqual(island.Width, x);
        Assert.AreEqual(island.Height, y);
    }
}
