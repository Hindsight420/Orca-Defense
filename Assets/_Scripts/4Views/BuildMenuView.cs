using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildMenuView : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;

    public void InitializeButtons(Action<BuildingBase> onButtonClicked)
    {
        foreach (BuildingBase buildingBase in DataSystem.Instance.BuildingBases)
        {
            if (buildingBase.name == "Destroy") continue;
            InitializeButton(buildingBase, () => onButtonClicked(buildingBase));
        }
    }

    void InitializeButton(BuildingBase buildingBase, UnityAction callback)
    {
        GameObject buttonGO = Instantiate(buttonPrefab, transform);
        buttonGO.name = $"Button - {buildingBase.name}";
        buttonGO.GetComponent<Button>().onClick.AddListener(callback);
        buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = buildingBase.name;
    }
}
