using System.Collections.Generic;

public interface IBuildingValidator
{
	List<string> ValidateBuildingPosition(Island map, BuildingTypeEnum buildingToBuild);

	List<string> ValidateResources(BuildingType buildingType);

	List<string> ValidateDestroyable(Island map);

	bool ShouldRenderRoof(Island map);

}
