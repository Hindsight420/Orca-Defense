using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ResourceList
{
    private readonly List<ResourceValue> resourceValueList;
    public List<ResourceValue> ResourceValueList { get => resourceValueList; }
    public int Count { get => resourceValueList.Count; }

    public ResourceList(List<ResourceValue> resources)
    {
        resourceValueList = resources;
    }

    public ResourceList() : this(new List<ResourceValue>()) { }

    public ResourceList(ResourceValue resource) : this(new List<ResourceValue>() { resource }) { }

    public ResourceValue TryGetResource(ResourceType type)
    {
        try { return resourceValueList.First(r => r.Type == type); }
        catch (InvalidOperationException) { return null; }
    }

    public void AddResource(ResourceValue resource)
    {
        ResourceValue targetResource = TryGetResource(resource.Type);
        if (targetResource != null) targetResource.Amount += resource;
        else resourceValueList.Add(resource);
    }

    public void AddResource(ResourceValue resource, ResourceValue amount)
    {
        ResourceValue targetResource = TryGetResource(resource.Type);
        if (targetResource == null)
        {
            targetResource = new(resource.Type);
            resourceValueList.Add(targetResource);
        }

        resource.TransferTo(targetResource, amount);
    }

    public void TransferTo(ResourceList target)
    {
        foreach (ResourceValue resourceValue in resourceValueList)
        {
            target.AddResource(resourceValue);
        }
    }

    public void TransferTo(ResourceList target, ResourceList amount)
    {
        foreach (ResourceValue resourceValue in amount.ResourceValueList)
        {
            target.AddResource(resourceValue);
        }
    }

    public bool CheckResourcesAvailability(ResourceList resources)
    {
        foreach(ResourceValue resource in resources.ResourceValueList)
        {
            if (TryGetResource(resource.Type) < resource.Amount)
                return false;
        }

        return true;
    }

    public ResourceList Minus(ResourceList subtrahends)
    {
        ResourceList difference = Copy();
        foreach(ResourceValue subtrahend in subtrahends.ResourceValueList)
        {
            ResourceValue minuend = difference.TryGetResource(subtrahend.Type);
            minuend.Amount -= subtrahend;
            if (minuend == 0) difference.ResourceValueList.Remove(minuend);
        }

        return difference;
    }

    public ResourceList Copy()
    {
        ResourceList copy = new();
        foreach(ResourceValue resource in ResourceValueList)
        {
            copy.AddResource(resource.Copy());
        }

        return copy;
    }

    public override bool Equals(object obj) => obj is ResourceList value ? Equals(value) : base.Equals(obj);

    public bool Equals(ResourceList target)
    {
        return !resourceValueList.Except(target.ResourceValueList).Any();
    }

    public override int GetHashCode() => HashCode.Combine(resourceValueList);

    public override string ToString()
    {
        string listOfResources = "\n";
        foreach (ResourceValue resource in resourceValueList)
        {
            listOfResources += resource;
            listOfResources += "\n";
        }
        return $"Resource list ({listOfResources})";

    }
}
