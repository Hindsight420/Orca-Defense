using EventCallbacks;
using NUnit.Framework;
using UnityEngine;

public class BuildingTests
{
    // TODO: initialize this variable
    [SerializeField]private BuildingBase buildingBase;

    [Test]
    [TestCase(4, 7)]
    public void TestBuildingConstructor(int x, int y)
    {
        Building building = new Building(x, y, buildingBase);

        Assert.AreEqual(building.BuildingBase, buildingBase);
        Assert.AreEqual(building.X, x);
        Assert.AreEqual(building.Y, y);
    }

    [Test]
    public void TestBuildingDestructor()
    {
        Building building = new Building(4, 7, buildingBase);
        bool eventTriggered = false;
        BuildingRemovedEvent.RegisterListener((BuildingRemovedEvent) => eventTriggered = true);
        building.Remove();

        Assert.IsTrue(eventTriggered);
    }
}
