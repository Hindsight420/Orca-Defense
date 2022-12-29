using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[TestFixture]
public class ResourceListTests
{
    private ResourceType woodType;
    private ResourceType fishType;
    private ResourceType stoneType;

    private Resource wood;
    private Resource fish;
    private Resource stone;

    private ResourceList empty;
    private ResourceList someWood;
    private ResourceList all;

    [SetUp]
    public void SetUpResourceTests()
    {
        woodType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        woodType.name = "Wood";
        fishType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        fishType.name = "Fish";
        stoneType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        stoneType.name = "Stone";

        wood = new(woodType, 100);
        fish = new(fishType, 30);
        stone = new(stoneType, 0);

        empty = new();
        someWood = new(wood);
        all = new(new List<Resource>() { wood, fish, stone });
    }

    [Test]
    public void Constructor()
    {
        ResourceList resourceList = new();
        Assert.AreEqual(new List<Resource>(), resourceList.Resources);
    }

    [Test]
    public void ConstructorWithValues()
    {
        List<Resource> listOfResources1 = new() { wood };
        List<Resource> listOfResources2 = new() { wood, fish };
        ResourceList resourceList1 = new(wood);
        ResourceList resourceList2 = new(listOfResources2);

        Assert.AreEqual(listOfResources1, resourceList1.Resources);
        Assert.AreEqual(listOfResources2, resourceList2.Resources);
    }

    [Test]
    public void TryGetResource()
    {
        Assert.AreEqual(wood, all.TryGetResource(woodType));
        Assert.AreEqual(fish, all.TryGetResource(fishType));
        Assert.AreEqual(stone, all.TryGetResource(stoneType));

        Assert.IsNull(someWood.TryGetResource(fishType));
    }

    [Test]
    public void Add()
    {
        someWood.Add(wood);
        someWood.Add(fish);

        Assert.AreEqual(200, someWood.TryGetResource(woodType).Amount);
        Assert.AreEqual(30, someWood.TryGetResource(fishType).Amount);
        Assert.IsNull(someWood.TryGetResource(stoneType));
    }

    [Test]
    public void TransferTo()
    {
        ResourceList someWoodCopy = someWood.Copy();
        someWood.TransferTo(empty);

        Assert.AreEqual(someWoodCopy, empty);
        Assert.AreEqual(new ResourceList(), someWood);
    }

    [Test]
    public void TransferToWithAmount()
    {
        ResourceList amount = new(new Resource(woodType, 40));
        all.TransferTo(empty, amount);

        Assert.AreEqual(40, empty.TryGetResource(woodType).Amount);
        Assert.IsNull(empty.TryGetResource(fishType));
        Assert.IsNull(empty.TryGetResource(stoneType));

        Assert.AreEqual(60, all.TryGetResource(woodType).Amount);
        Assert.AreEqual(30, all.TryGetResource(fishType).Amount);
        Assert.IsNull(all.TryGetResource(stoneType));
    }

    [Test]
    public void CleanUp()
    {
        all.CleanUp();

        Assert.IsNull(all.TryGetResource(stoneType));
    }

    [Test]
    public void CheckResourcesAvailability()
    {
        Assert.IsTrue(all.CheckResourcesAvailability(someWood));
        Assert.IsFalse(someWood.CheckResourcesAvailability(all));
        Assert.IsTrue(someWood.CheckResourcesAvailability(empty));
    }

    [Test]
    public void Minus()
    {
        ResourceList difference = all.Minus(someWood);

        Assert.IsNull(difference.TryGetResource(woodType));
        Assert.AreEqual(fish, difference.TryGetResource(fishType));
        Assert.IsNull(difference.TryGetResource(stoneType));
    }

    [Test]
    public void Equals()
    {
        Assert.AreEqual(someWood, new ResourceList(wood));
        Assert.IsTrue(someWood.Equals(new ResourceList(wood)));
        Assert.IsTrue(someWood == new ResourceList(wood));
        Assert.IsTrue(someWood != all);
    }
}
