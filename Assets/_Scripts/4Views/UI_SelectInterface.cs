using Assets._Scripts._1Data;
using Assets._Scripts.Utility;
using UnityEngine;

public class UI_SelectInterface : Singleton<UI_SelectInterface>
{
    private Logger _logger;
    private Transform _targetSelection;
    private RectTransform _selectionTransform;
    [SerializeField]
    private UI_SelectionPanel _selectionPanel;

    private WidthHeight PenguinSize = new WidthHeight(3, 5);
    private WidthHeight BuildingSize = new WidthHeight(2.5f,2.5f);

    protected override void Awake()
    {
        base.Awake();
        _logger = Logger.Instance;
        _selectionTransform = GetComponent<RectTransform>();
        DisableSelf();
    }

    private void OnMouseDown()
    {
        if (_targetSelection != null)
        {
            DisableSelf();
            _selectionPanel.DisableInfoPanel();
        }
    }

    public void SelectEntity<T>(T selectedObject) where T : ISelectionData
    {
        switch (selectedObject)
        {
            case PenguinData: SelectPenguin(selectedObject); break;
            case FishStockpileData: SelectStockpile(selectedObject); break;
        }

        _selectionPanel.ConstructInfoPanel(selectedObject);
    }

    private void SelectStockpile(ISelectionData stockpileData)
    {
        if (stockpileData is not FishStockpileData) { _logger.LogError("Attempted to select stockpile without StockpileData"); return; }
        FishStockpileData data = (FishStockpileData)stockpileData;

        //_logger.LogDebug($"Penguin {data.Name} Selected");
        _selectionTransform.sizeDelta = new Vector2(BuildingSize.Width, BuildingSize.Height);
        _targetSelection = data.GetParent();

        EnableSelf(0.5f, 0.5f);
    }

    private void SelectPenguin(ISelectionData penguinData)
    { 
        if (penguinData is not PenguinData) { _logger.LogError("Attempted to select penguin without PenguinData"); return; }
        PenguinData data = (PenguinData)penguinData;

        //_logger.LogDebug($"Penguin {data.Name} Selected");
        _selectionTransform.sizeDelta = new Vector2(PenguinSize.Width, PenguinSize.Height);
        _targetSelection = data.GetParent();

        EnableSelf();
    }

    private void EnableSelf (float offsetX = 0, float offsetY = 0)
    {
        if (_targetSelection != null)
        {
            gameObject.SetActive(true);
            _selectionTransform.SetParent(_targetSelection);
            _selectionTransform.localPosition = new Vector3(offsetX, offsetY, 9f);

        }
    }
    private void DisableSelf ()
    {
        gameObject.SetActive(false);
        _selectionTransform.SetParent(null);
    }
}