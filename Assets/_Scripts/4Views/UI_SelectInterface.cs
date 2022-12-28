using Assets._Scripts._1Data;
using Assets._Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectInterface : Singleton<UI_SelectInterface>
{
    private Logger _logger;
    private Transform _targetSelection;
    private RectTransform _selectionTransform;
    [SerializeField]
    private UI_SelectionPanel _selectionPanel;

    private WidthHeight PenguinSize = new WidthHeight(3, 5);

    protected override void Awake()
    {
        base.Awake();
        _logger = Logger.Instance;
        _selectionTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        if (_selectionTransform != null)
        {
            DisableSelf();
            _selectionPanel.DisableInfoPanel();
        }
    }

    public void SelectEntity<T>(T selectedObject) where T : SelectableData
    {
        switch (selectedObject)
        {
            case PenguinData: SelectPenguin(selectedObject); break;
        }

        _selectionPanel.ConstructInfoPanel(selectedObject);
    }

    public void SelectPenguin(SelectableData penguinData)
    { 
        if (penguinData is not PenguinData) { _logger.LogError("Attempted to select penguin without PenguinData"); return; }
        PenguinData data = (PenguinData)penguinData;

        //_logger.LogDebug($"Penguin {data.Name} Selected");
        _selectionTransform.sizeDelta = new Vector2(PenguinSize.Width, PenguinSize.Height);
        _targetSelection = data.ParentTransform;

        EnableSelf();
    }

    private void EnableSelf ()
    {
        if (_targetSelection != null)
        {
            gameObject.SetActive(true);
            _selectionTransform.SetParent(_targetSelection);
            _selectionTransform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private void DisableSelf ()
    {
        gameObject.SetActive(false);
        _selectionTransform.SetParent(null);
    }

     

}

public enum UI_SelectType
{
    Penguin
}
