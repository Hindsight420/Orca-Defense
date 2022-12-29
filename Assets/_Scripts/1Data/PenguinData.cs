using UnityEngine;

namespace Assets._Scripts._1Data
{
    public class PenguinData : SelectionData
    {
        public PenguinData (Transform penguinTransform)
        { 
            Title = "Pengu";
            Description = "Ice Fisher";
            ParentTransform = penguinTransform;
        }

        public string GetDescriptionOfPenguin ()
        {
            return Description;
        }
    }
}
