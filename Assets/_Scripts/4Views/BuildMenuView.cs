using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildMenuView : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;

    public void InitializeButtons(Action<BuildingType> onButtonClicked)
    {
        foreach (BuildingType buildingType in DataSystem.Instance.BuildingTypes)
        {
            if (buildingType.name == "Destroy") continue;
            InitializeButton(buildingType, () => onButtonClicked(buildingType));
        }
    }

    void InitializeButton(BuildingType buildingType, UnityAction callback)
    {
        GameObject buttonGO = Instantiate(buttonPrefab, transform);
        buttonGO.name = $"Button - {buildingType}";
        buttonGO.GetComponent<Button>().onClick.AddListener(callback);
        buttonGO.GetComponentsInChildren<Image>()[1].sprite = buildingType.Icon;
    }
}
