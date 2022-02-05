using NUnit.Framework;

public class IslandTests
{
    [Test]
    public void TestIslandConstructor()
    {
        Island island = new Island(15, 10);

        Assert.IsTrue(island.Width == 15);
        Assert.IsTrue(island.Height == 10);
    }
}
