using OrcaDefense.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private BuildingTypeEnum buildingEnum;
    [SerializeField]
    private bool hasRoof;
    [SerializeField]
    private List<ResourceValue> cost;
    [SerializeField]
    private List<ResourceValue> income;
    [SerializeField]
    private int? ticksPerIncome;

    //Accessors for the above
    public GameObject Prefab { get => prefab; }
    public Sprite Icon { get => icon; }
    public BuildingTypeEnum BuildingEnum { get => buildingEnum; }
    public bool HasRoof { get => hasRoof; }
    public List<ResourceValue> Cost { get => cost; }
    public List<ResourceValue> Income { get => income; }
    public int? TicksPerIncome { get => ticksPerIncome; }


    public IBuildingValidator GetBuildingValidator(Tile t)
    {
        return GetValidatorByBuildingType(buildingEnum, t);
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

