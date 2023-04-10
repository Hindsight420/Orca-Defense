using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ResourceTests
{
    [SerializeField]
    private ResourceType _resourceType;

    [OneTimeSetUp]
    public void SetUpResourceTests()
    {
        _resourceType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        _resourceType.name = "Wood";
    }

    [Test]
    [TestCase(0)]
    [TestCase(13)]
    [TestCase(-5)]
    public void TestResourceToIntImplicit(int originalAmount)
    {
        int amount = originalAmount;
        if (amount < 0)
        {
            LogAssert.Expect(LogType.Error, $"Resource (Wood: 0) tried to set to {amount}, setting amount to 0 instead");
            amount = 0;
        }

        Resource r = new(_resourceType, originalAmount);
        Assert.AreEqual(amount, r.Amount);

        int i = r;
        Assert.AreEqual(amount, i);
    }

    [Test]
    public void Equals()
    {
        Resource
            resource1 = new(_resourceType, 80),
            resource2 = new(_resourceType, 80),
            resource3 = new(_resourceType, 100);

        Assert.AreEqual(resource1, resource2);
        Assert.IsTrue(resource1 != resource3);
    }
}
