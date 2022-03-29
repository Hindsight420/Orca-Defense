using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    public GameObject Prefab;

    public List<ResourceValue> Cost;

    public bool hasRoof;

    public override string ToString()
    {
        return $"Building Type: {name}";
    }
}
