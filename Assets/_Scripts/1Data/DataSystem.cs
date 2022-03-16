using System.Collections.Generic;
using System.Linq;

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
        BuildingBases = UnityEngine.Resources.LoadAll<BuildingBase>("BuildingBases").ToList();
    }

    public BuildingBase GetBuildingBase(string name)
    {
        return BuildingBases.Single(b => b.name == name);
    }
}
