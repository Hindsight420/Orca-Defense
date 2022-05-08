using OrcaDefense.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class FishingHutValidator: BaseBuildingValidator, IBuildingValidator
{
	public FishingHutValidator(Tile tile) : base (tile)
	{
		
	}

	public override string[] ValidatePosition(Island map)
	{
		var errors = new List<string>();
		errors.AddRange(base.ValidatePosition(map));
		if (_tile.Y != 0) { errors.Add("Fishing huts must be built on the ice!"); }

		return errors.ToArray();
	}
}
