using OrcaDefense.Models;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    [SerializeField]
    private GameObject _buildingPrefab;
    [SerializeField]
    private GameObject _previewPrefab;

    [SerializeField]
    private BuildingTypeEnum _buildingEnum;
    [SerializeField]
    private bool _hasRoof;
    [SerializeField]
    private ResourceList _cost;
    [SerializeField]
    private ResourceList _income;
    [SerializeField]
    private int _ticksPerIncome;

    //Accessors for the above
    public GameObject BuildingPrefab { get => _buildingPrefab; }
    public GameObject PreviewPrefab { get => _previewPrefab; }
    public BuildingTypeEnum BuildingEnum { get => _buildingEnum; }
    public bool HasRoof { get => _hasRoof; }
    public ResourceList Cost { get => _cost; }
    public ResourceList Income { get => _income; }
    public int TicksPerIncome { get => _ticksPerIncome; }


    public IBuildingValidator GetBuildingValidator(Tile t)
    {
        return GetValidatorByBuildingType(_buildingEnum, t);
    }

    public override string ToString()
    {
        return $"Building Type: {name}";
    }

    private IBuildingValidator GetValidatorByBuildingType(BuildingTypeEnum type, Tile t)
    {
        return type switch
        {
            BuildingTypeEnum.FishingHut => new FishingHutValidator(t),
            _ => new BaseBuildingValidator(t),
        };
    }
}

[Serializable]
public enum BuildingTypeEnum
{
    FishingHut,
    Default,
}

