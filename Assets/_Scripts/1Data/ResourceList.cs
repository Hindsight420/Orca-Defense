using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ResourceList
{
    private readonly List<ResourceValue> resourceValueList;
    public List<ResourceValue> ResourceValueList { get => resourceValueList; }

    public ResourceList() : this(new()) { }

    public ResourceList(List<ResourceValue> resourceValues)
    {
        resourceValueList = resourceValues;
    }

    public ResourceValue GetResource(ResourceType type)
    {
        return resourceValueList.First(r => r.Type == type);
    }

    public void TransferTo(ResourceList target, ResourceList resources)
    {
        foreach (ResourceValue resourceValue in resources.ResourceValueList)
        {
            ResourceType type = resourceValue.Type;
            ResourceValue thisResource = GetResource(type);
            ResourceValue targetResource = target.GetResource(type);
            thisResource.TransferTo(targetResource, resourceValue.Amount);
        }
    }

    public void TransferTo(ResourceList target)
    {
        TransferTo(target, this);
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
        foreach(ResourceValue resource in resourceValueList)
        {
            listOfResources += resource;
            listOfResources += "\n";
        }
        return $"Resource list ({listOfResources})";

    }
}
