using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Base")]
public class BuildingBase : ScriptableObject
{
    public List<ResourceValue> Cost;
    public GameObject Prefab;
}
