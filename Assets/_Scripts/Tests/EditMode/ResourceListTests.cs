using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[TestFixture]
public class ResourceListTests
{
    private ResourceType _woodType;
    private ResourceType _fishType;
    private ResourceType _stoneType;

    private Resource _wood;
    private Resource _fish;
    private Resource _stone;

    private ResourceList _empty;
    private ResourceList _someWood;
    private ResourceList _all;

    [SetUp]
    public void SetUpResourceTests()
    {
        _woodType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        _woodType.name = "Wood";
        _fishType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        _fishType.name = "Fish";
        _stoneType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        _stoneType.name = "Stone";

        _wood = new(_woodType, 100);
        _fish = new(_fishType, 30);
        _stone = new(_stoneType, 0);

        _empty = new();
        _someWood = new(_wood);
        _all = new(new List<Resource>() { _wood, _fish, _stone });
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
        List<Resource> listOfResources1 = new() { _wood };
        List<Resource> listOfResources2 = new() { _wood, _fish };
        ResourceList resourceList1 = new(_wood);
        ResourceList resourceList2 = new(listOfResources2);

        Assert.AreEqual(listOfResources1, resourceList1.Resources);
        Assert.AreEqual(listOfResources2, resourceList2.Resources);
    }

    [Test]
    public void TryGetResource()
    {
        Assert.AreEqual(_wood, _all.TryGetResource(_woodType));
        Assert.AreEqual(_fish, _all.TryGetResource(_fishType));
        Assert.AreEqual(_stone, _all.TryGetResource(_stoneType));

        Assert.IsNull(_someWood.TryGetResource(_fishType));
    }

    [Test]
    public void Add()
    {
        _someWood.Add(_wood);
        _someWood.Add(_fish);

        Assert.AreEqual(200, _someWood.TryGetResource(_woodType).Amount);
        Assert.AreEqual(30, _someWood.TryGetResource(_fishType).Amount);
        Assert.IsNull(_someWood.TryGetResource(_stoneType));
    }

    [Test]
    public void TransferTo()
    {
        ResourceList someWoodCopy = _someWood.Copy();
        _someWood.TransferTo(_empty);

        Assert.AreEqual(someWoodCopy, _empty);
        Assert.AreEqual(new ResourceList(), _someWood);
    }

    [Test]
    public void TransferToWithAmount()
    {
        ResourceList amount = new(new Resource(_woodType, 40));
        _all.TransferTo(_empty, amount);

        Assert.AreEqual(40, _empty.TryGetResource(_woodType).Amount);
        Assert.IsNull(_empty.TryGetResource(_fishType));
        Assert.IsNull(_empty.TryGetResource(_stoneType));

        Assert.AreEqual(60, _all.TryGetResource(_woodType).Amount);
        Assert.AreEqual(30, _all.TryGetResource(_fishType).Amount);
        Assert.IsNull(_all.TryGetResource(_stoneType));
    }

    [Test]
    public void CleanUp()
    {
        _all.CleanUp();

        Assert.IsNull(_all.TryGetResource(_stoneType));
    }

    [Test]
    public void CheckResourcesAvailability()
    {
        Assert.IsTrue(_all.CheckResourcesAvailability(_someWood));
        Assert.IsFalse(_someWood.CheckResourcesAvailability(_all));
        Assert.IsTrue(_someWood.CheckResourcesAvailability(_empty));
    }

    [Test]
    public void Minus()
    {
        ResourceList difference = _all.Minus(_someWood);

        Assert.IsNull(difference.TryGetResource(_woodType));
        Assert.AreEqual(_fish, difference.TryGetResource(_fishType));
        Assert.IsNull(difference.TryGetResource(_stoneType));
    }

    [Test]
    public void Equals()
    {
        Assert.AreEqual(_someWood, new ResourceList(_wood));
        Assert.IsTrue(_someWood.Equals(new ResourceList(_wood)));
        Assert.IsTrue(_someWood == new ResourceList(_wood));
        Assert.IsTrue(_someWood != _all);
    }
}
