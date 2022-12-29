using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    [SerializeField]
    private DataEntity _dataEntity;

    public void OnMouseDown()
    {
        if (_dataEntity == null)
        {
            Logger.Instance.LogError($"[{gameObject.name}:{gameObject.GetHashCode()}] IDataEntity has not been added via inspector. Failed to select object");
            return;
        }

        UI_SelectInterface.Instance.SelectEntity(_dataEntity.GetSelectionData());
    }
}
