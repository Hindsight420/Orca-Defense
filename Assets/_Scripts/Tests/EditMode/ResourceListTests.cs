using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[TestFixture]
public class ResourceListTests
{
    private ResourceType woodType;
    private ResourceType fishType;
    private ResourceType stoneType;

    private ResourceValue wood;
    private ResourceValue fish;
    private ResourceValue stone;

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
        all = new(new List<ResourceValue>() { wood, fish, stone });
    }

    [Test]
    public void Constructor()
    {
        ResourceList resourceList = new();
        Assert.AreEqual(new List<ResourceValue>(), resourceList.ResourceValueList);
    }

    [Test]
    public void ConstructorWithValues()
    {
        List<ResourceValue> listOfResources1 = new() { wood };
        List<ResourceValue> listOfResources2 = new() { wood, fish };
        ResourceList resourceList1 = new(wood);
        ResourceList resourceList2 = new(listOfResources2);

        Assert.AreEqual(listOfResources1, resourceList1.ResourceValueList);
        Assert.AreEqual(listOfResources2, resourceList2.ResourceValueList);
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
        ResourceList amount = new(new ResourceValue(woodType, 40));
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
