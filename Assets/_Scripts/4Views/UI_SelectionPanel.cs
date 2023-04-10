using Assets._Scripts._1Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SelectionPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject descValuePairPrefab;
    [SerializeField]
    private GameObject descPrefab;
    [SerializeField]
    private TextMeshProUGUI _title;
    private Logger _logger;

    private List<GameObject> descriptions;

    private void Awake()
    {
        _logger = Logger.Instance;
        descriptions = new List<GameObject>();
        gameObject.SetActive(false);
    }

    public void ConstructInfoPanel<T>(T selectedObject) where T : ISelectionData
    {
        ClearInfoPanel();

        switch (selectedObject)
        {
            case PenguinData: ConstructPenguinInfoPanel(selectedObject); break;
            case FishStockpileData: ConstructStockpileInfoPanel(selectedObject); break;
        }

        EnableInfoPanel();
    }

    public void DisableInfoPanel()
    {
        ClearInfoPanel();
        gameObject.SetActive(false);
    }

    private void ClearInfoPanel()
    {
        descriptions.ForEach(x => Destroy(x));
        descriptions = new List<GameObject>();
    }

    private void EnableInfoPanel()
    {
        //Place each description
        var currentYOffset = 100f;
        for (var i = 0; i < descriptions.Count; i++)
        {
            descriptions[i].transform.Translate(new Vector3(0, currentYOffset, 0));
            //We shouldn't need to null check this
            currentYOffset -= descriptions[i].GetComponentInChildren<RectTransform>().rect.height;
        }

        gameObject.SetActive(true);
    }

    private void ConstructPenguinInfoPanel(ISelectionData penguinData)
    {
        if (penguinData is not PenguinData) { _logger.LogError("Attempted to select penguin without PenguinData"); return; }
        PenguinData data = (PenguinData)penguinData;

        _title.text = data.GetTitle();
        var description = Instantiate(descPrefab, transform);
        var descriptionText = description.GetComponentInChildren<TextMeshProUGUI>();
        descriptionText.text = data.GetDescriptionOfPenguin();
        descriptions.Add(description);
    }

    private void ConstructStockpileInfoPanel(ISelectionData stockpileData)
    {
        if (stockpileData is not FishStockpileData) { _logger.LogError("Attempted to select penguin without PenguinData"); return; }
        FishStockpileData data = (FishStockpileData)stockpileData;

        _title.text = data.GetTitle();

        //e.g. Max Capacity: 50
        var description = Instantiate(descPrefab, transform);
        var descriptionText = description.GetComponentInChildren<TextMeshProUGUI>();
        descriptionText.text = data.GetCapacityDescription();
        descriptions.Add(description);

        // e.g. Current Quantity 3/50 fish
        description = Instantiate(descValuePairPrefab, transform); // Title
        descriptionText = description.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>();
        descriptionText.text = "Current Quantity";
        
        descriptionText = description.transform.Find("Value").GetComponentInChildren<TextMeshProUGUI>(); // Value
        descriptionText.text = data.GetStockpileCountDescription();
        descriptions.Add(description);

        //e.g. A stockpile used to hold fish.
        description = Instantiate(descPrefab, transform);
        descriptionText = description.GetComponentInChildren<TextMeshProUGUI>();
        descriptionText.text = data.GetStockpileDescription();
        descriptions.Add(description);
    }
}
