using EventCallbacks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTests
{
    // TODO: initialize this variable
    [SerializeField]private BuildingBase buildingBase;

    [OneTimeSetUp]
    public void SetUpBuildingTests()
    {
        buildingBase = (BuildingBase)ScriptableObject.CreateInstance(nameof(BuildingBase));
        buildingBase.name = "TestBuilding";

        buildingBase.Cost = new List<ResourceValue>();
        ResourceType resourceType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        resourceType.name = "Wood";
        buildingBase.Cost.Add(new ResourceValue(resourceType, 50));
    }

    [Test]
    [TestCase(4, 7)]
    public void TestBuildingConstructor(int x, int y)
    {
        Building building = new(x, y, buildingBase);

        Assert.AreEqual(building.BuildingBase, buildingBase);
        Assert.AreEqual(building.X, x);
        Assert.AreEqual(building.Y, y);
    }

    [Test]
    public void TestBuildingDestructor()
    {
        Building building = new(4, 7, buildingBase);
        bool eventTriggered = false;
        BuildingRemovedEvent.RegisterListener((BuildingRemovedEvent) => eventTriggered = true);
        building.Remove();

        Assert.IsTrue(eventTriggered);
    }
}
