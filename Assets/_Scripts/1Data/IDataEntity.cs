using Assets._Scripts._1Data;
using UnityEngine;

public interface IDataEntity 
{
    public ISelectionData GetSelectionData();
}

public abstract class DataEntity : MonoBehaviour, IDataEntity
{
    public abstract ISelectionData GetSelectionData();
} 
