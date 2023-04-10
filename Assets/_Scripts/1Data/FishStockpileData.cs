using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts._1Data
{
    public class FishStockpileData : SelectionData, IStockpileData
    {
        private FishStockpile _fishStockpile;

        public FishStockpileData(FishStockpile fishStockpile)
        {
            _fishStockpile = fishStockpile;
            Title = "Fish Stockpile";
            Description = $"Holds Maximum {_fishStockpile.Capacity} Fish";
            ParentTransform = fishStockpile.transform;
        }

        public string GetStockpileCountDescription()
        {
            return $"{_fishStockpile.CurrentQuantity}/{_fishStockpile.Capacity}";
        }

        public string GetCapacityDescription ()
        {
            return $"Max Capacity: {_fishStockpile.Capacity}";
        }

        public string GetStockpileDescription()
        {
            return "A stockpile that holds fish for Penguins to eat";
        }
    }
}
