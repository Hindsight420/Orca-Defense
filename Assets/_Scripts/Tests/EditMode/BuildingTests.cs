using EventCallbacks;
using NUnit.Framework;
using OrcaDefense.Models;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTests
{
    // TODO: initialize this variable
    [SerializeField] private BuildingType buildingType;

    [OneTimeSetUp]
    public void SetUpBuildingTests()
    {
        buildingType = (BuildingType)ScriptableObject.CreateInstance(nameof(BuildingType));
        buildingType.name = "TestBuilding";

        ResourceType resourceType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        resourceType.name = "Wood";
        buildingType.Cost.Add(new ResourceValue(resourceType, 50));
    }

    [Test]
    [TestCase(4, 7)]
    public void TestBuildingConstructor(int x, int y)
    {
        Building building = new(buildingType, new Tile(x, y));

        Assert.AreEqual(building.BuildingType, buildingType);
        Assert.AreEqual(building.X, x);
        Assert.AreEqual(building.Y, y);
    }
}
