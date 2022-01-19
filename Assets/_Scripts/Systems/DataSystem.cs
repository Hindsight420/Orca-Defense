using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataSystem : Singleton<DataSystem>
{
    public List<BuildingSettings> BuildingSettings { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        AssembleData();
    }

    private void AssembleData()
    {
        BuildingSettings = Resources.LoadAll<BuildingSettings>("Buildings").ToList();
    }
}
