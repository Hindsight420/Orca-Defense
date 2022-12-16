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

    public void Add(ResourceValue resource)
    {
        ResourceValue targetResource = TryGetResource(resource.Type);
        if (targetResource != null) targetResource.Amount += resource;
        else resourceValueList.Add(resource.Copy());
    }

    public void TransferTo(ResourceList target)
    {
        TransferTo(target, this);
    }

    public void TransferTo(ResourceList target, ResourceList amount)
    {
        foreach (ResourceValue resourceAmount in amount.ResourceValueList)
        {
            ResourceValue resourceValue = TryGetResource(resourceAmount.Type);
            if (resourceAmount == null) continue;

            resourceValue.TransferTo(target, resourceAmount);
        }

        CleanUp();
    }

    public void CleanUp()
    {
        ResourceValueList.RemoveAll(r => r.Amount == 0);
    }

    public bool CheckResourcesAvailability(ResourceList amount)
    {
        foreach(ResourceValue a in amount.ResourceValueList)
        {
            ResourceValue resource = TryGetResource(a.Type);
            if (resource == null || resource < a.Amount)
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
        }

        difference.CleanUp();
        return difference;
    }

    public ResourceList Copy()
    {
        ResourceList copy = new();
        foreach(ResourceValue resource in ResourceValueList)
        {
            copy.Add(resource.Copy());
        }

        return copy;
    }

    public bool Equals(ResourceList target)
    {
        return resourceValueList.SequenceEqual(target.ResourceValueList);
    }

    public override bool Equals(object obj) => obj is ResourceList value ? Equals(value) : base.Equals(obj);

    public static bool operator ==(ResourceList resourceList1, ResourceList resourceList2)
    {
        return resourceList1.Equals(resourceList2);
    }
    
    public static bool operator !=(ResourceList resourceList1, ResourceList resourceList2)
    {
        return !resourceList1.Equals(resourceList2);
    }

    public override int GetHashCode() => HashCode.Combine(resourceValueList);

    public override string ToString()
    {
        return string.Join(Environment.NewLine, resourceValueList);
    }
}
