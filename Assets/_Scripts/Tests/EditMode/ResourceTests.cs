using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

public class ResourceTests
{
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

        Resource wood1 = new(ResourceType.Wood, originalAmount);
        Assert.AreEqual(amount, wood1.Amount);

        int i = wood1;
        Assert.AreEqual(amount, i);
    }
}
