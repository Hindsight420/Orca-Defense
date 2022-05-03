using OrcaDefense.Models;
using System;
using System.Linq;

public class FishingHutValidator: BaseBuildingValidator, IBuildingValidator
{
	public FishingHutValidator()
	{
		
	}

	public override string[] ValidateAdjacency(BuildingTypeEnum buildingToBuild)
	{
		return buildingToBuild == BuildingTypeEnum.FishingHut ? 
			new string[] { "Testing two next to each other" } :
			null;
	}
}
