﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceList
{
    [SerializeField]
    private List<Resource> _resources;

    public List<Resource> Resources { get => _resources; }
    public int Count { get => _resources.Count; }

    public ResourceList(List<Resource> resources)
    {
        _resources = resources;
    }

    public ResourceList() : this(new List<Resource>()) { }

    public ResourceList(Resource resource) : this(new List<Resource>() { resource }) { }

    public Resource TryGetResource(ResourceType type)
    {
        try { return _resources.First(r => r.Type == type); }
        catch (InvalidOperationException) { return null; }
    }

    public void Add(Resource resource)
    {
        Resource targetResource = TryGetResource(resource.Type);
        if (targetResource != null) targetResource.Amount += resource;
        else _resources.Add(resource.Copy());
    }

    public void Add(ResourceList resources)
    {
        foreach (Resource resource in resources.Resources) 
            Add(resource);
    }

    public void TransferTo(ResourceList target)
    {
        TransferTo(target, this);
    }

    public void TransferTo(ResourceList target, ResourceList amount)
    {
        foreach (Resource resourceAmount in amount.Resources)
        {
            Resource resource = TryGetResource(resourceAmount.Type);
            if (resourceAmount == null) continue;

            resource.TransferTo(target, resourceAmount);
        }

        CleanUp();
    }

    public void CleanUp()
    {
        Resources.RemoveAll(r => r.Amount == 0);
    }

    public bool CheckResourcesAvailability(ResourceList amount)
    {
        foreach (Resource a in amount.Resources)
        {
            Resource resource = TryGetResource(a.Type);
            if (resource == null || resource < a.Amount)
                return false;
        }

        return true;
    }

    public ResourceList Minus(ResourceList subtrahends)
    {
        ResourceList difference = Copy();
        foreach (Resource subtrahend in subtrahends.Resources)
        {
            Resource minuend = difference.TryGetResource(subtrahend.Type);
            minuend.Amount -= subtrahend;
        }

        difference.CleanUp();
        return difference;
    }

    public ResourceList Copy()
    {
        ResourceList copy = new();
        foreach (Resource resource in Resources)
        {
            copy.Add(resource.Copy());
        }

        return copy;
    }

    public bool Equals(ResourceList target)
    {
        return _resources.SequenceEqual(target.Resources);
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

    public override int GetHashCode() => HashCode.Combine(_resources);

    public override string ToString()
    {
        return string.Join(Environment.NewLine, _resources);
    }
}