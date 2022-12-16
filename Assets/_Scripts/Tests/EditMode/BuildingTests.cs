using NUnit.Framework;
using OrcaDefense.Models;
using UnityEngine;

public class BuildingTests
{
    // TODO: initialize this variable
    [SerializeField] private BuildingType buildingType;

    [OneTimeSetUp]
    public void SetUpBuildingTests()
    {
        buildingType = ScriptableObject.CreateInstance<BuildingType>();
        buildingType.name = "TestBuilding";

        ResourceType resourceType = ScriptableObject.CreateInstance<ResourceType>();
        resourceType.name = "Wood";
        //buildingType.Cost.Add(new Resource(resourceType, 50));
    }

    //[Test]
    //[TestCase(4, 7)]
    //public void TestBuildingConstructor(int x, int y)
    //{
    //    Building building = new(buildingType, new Tile(x, y));

    //    Assert.AreEqual(building.Type, buildingType);
    //    Assert.AreEqual(building.X, x);
    //    Assert.AreEqual(building.Y, y);
    //}
}
