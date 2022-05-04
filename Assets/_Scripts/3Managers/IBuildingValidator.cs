public interface IBuildingValidator
{
	//Does this building care if we build it next to it's neighbours?
	string[] ValidateAdjacencies(Island map, int x, int y);

	//Does this building care if we build X building next to it?
	string[] ValidateAdjacency(BuildingTypeEnum buildingToBuild);

	//Can the given building be built on top?
	string[] ValidateCanBuildOnTop(Island map, int x, int y);

	//Should this building render a roof?
	bool ShouldRenderRoof(Island map, int x, int y);

}
