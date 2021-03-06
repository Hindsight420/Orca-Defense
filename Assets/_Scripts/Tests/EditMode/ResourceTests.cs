using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ResourceTests
{
    [SerializeField] private ResourceType resourceType;

    [OneTimeSetUp]
    public void SetUpResourceTests()
    {
        resourceType = (ResourceType)ScriptableObject.CreateInstance(nameof(ResourceType));
        resourceType.name = "Wood";
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

        ResourceValue r = new(resourceType, originalAmount);
        Assert.AreEqual(amount, r.Amount);

        int i = r;
        Assert.AreEqual(amount, i);
    }
}
