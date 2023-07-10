using UnityEngine;

namespace Assets._Scripts._1Data
{
    public interface ISelectionData
    {
        public string GetTitle();
        public Transform GetParent();
    }

    public abstract class SelectionData : ISelectionData
    {
        protected string Title { get; set; }
        protected string Description { get; set; }
        protected Transform ParentTransform { get; set; }

        public string GetTitle() => Title;
        public Transform GetParent() => ParentTransform;
    }
}
