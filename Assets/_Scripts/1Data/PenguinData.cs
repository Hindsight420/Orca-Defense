using UnityEngine;

namespace Assets._Scripts._1Data
{
    public class PenguinData : SelectableData
    {
        public PenguinData (Transform penguinTransform)
        { 
            Name = "Pengu";
            Description = "Ice Fisher";
            ParentTransform = penguinTransform;
        }
    }
}
