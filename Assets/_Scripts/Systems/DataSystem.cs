using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataSystem : Singleton<DataSystem>
{
    public List<BuildingBase> BuildingBases { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        AssembleData();
    }

    private void AssembleData()
    {
        BuildingBases = Resources.LoadAll<BuildingBase>("BuildingBases").ToList();
    }
}
