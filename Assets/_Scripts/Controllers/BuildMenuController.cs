using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour
{
    public GameObject buttonPrefab;

    void Start()
    {
        CreateButtons();
    }

    void CreateButtons()
    {
        foreach (BuildingBase buildingBase in DataSystem.Instance.BuildingBases)
        {
            if (buildingBase.name == "Destroy") continue;
            CreateButton(buildingBase);
        }
    }

    void CreateButton(BuildingBase buildingBase)
    {
        GameObject buttonGO = Instantiate(buttonPrefab, transform);
        buttonGO.name = $"Button - {buildingBase.name}";
        buttonGO.GetComponent<Button>().onClick.AddListener(() => OnBuildingButtonClicked(buildingBase));
        buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = buildingBase.name;
    }


    public void OnBuildingButtonClicked(BuildingBase buildingBase)
    {
        GameManager.Instance.SelectedBuilding = buildingBase;
        GameManager.Instance.UpdateGameState(GameState.Build);
    }

    public void OnDestroyButtonClicked()
    {
        // TODO: Clean up this string reference
        GameManager.Instance.SelectedBuilding = DataSystem.Instance.GetBuildingBase("Destroy");
        GameManager.Instance.UpdateGameState(GameState.Destroy);
    }
}
