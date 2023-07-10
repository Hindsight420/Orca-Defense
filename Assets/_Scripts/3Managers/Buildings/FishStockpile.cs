using Assets._Scripts._1Data;
using UnityEngine;

public class FishStockpile : Stockpile<Fish>
{
    private FishStockpileData _fishStockpileData;

    private void Awake()
    {
        ConfigureStore(50);
        _fishStockpileData = new FishStockpileData(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddToStore();
        } 
    }

    public void OnMouseDown()
    {
        UI_SelectInterface.Instance.SelectEntity(_fishStockpileData);
    }

    public override ISelectionData GetSelectionData()
    {
        return _fishStockpileData;
    }
}
