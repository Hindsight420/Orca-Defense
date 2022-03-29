using System.Collections.Generic;
using System.Linq;

public class DataSystem : Singleton<DataSystem>
{
    public List<BuildingType> BuildingTypes { get; private set; }
    public List<ResourceType> ResourceTypes { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        AssembleData();
    }

    private void AssembleData()
    {
        BuildingTypes = UnityEngine.Resources.LoadAll<BuildingType>("BuildingTypes").ToList();
        ResourceTypes = UnityEngine.Resources.LoadAll<ResourceType>("ResourceTypes").ToList();
    }

    public BuildingType GetBuildingType(string name)
    {
        return BuildingTypes.Single(b => b.name == name);
    }

    public ResourceType GetResourceType(string name)
    {
        return ResourceTypes.Single(b => b.name == name);
    }
}
