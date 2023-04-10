using Assets._Scripts._1Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStockpile : Stockpile<Fish>
{
    private FishStockpileData fishStockpileData;

    private void Awake()
    {
        ConfigureStore(50);
        fishStockpileData = new FishStockpileData(this);
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
        UI_SelectInterface.Instance.SelectEntity(fishStockpileData);
    }

    public override ISelectionData GetSelectionData()
    {
        return fishStockpileData;
    }
}
