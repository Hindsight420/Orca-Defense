using EventCallbacks;
using NUnit.Framework;
using UnityEngine;

public class BuildingTests
{
    // TODO: initialize this variable
    [SerializeField]private BuildingBase buildingBase;

    [Test]
    public void TestBuildingConstructor()
    {
        Building building = new Building(4, 7, buildingBase);

        Assert.IsTrue(building.BuildingBase == buildingBase);
        Assert.IsTrue(building.X == 4);
        Assert.IsTrue(building.Y == 7);
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
