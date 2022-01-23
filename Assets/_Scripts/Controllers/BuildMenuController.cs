using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour
{
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateButtons()
    {
        foreach (BuildingBase buildingBase in DataSystem.Instance.BuildingBases)
            CreateButton(buildingBase);
    }

    void CreateButton(BuildingBase buildingBase)
    {
        GameObject buttonGO = Instantiate(buttonPrefab, transform);
        buttonGO.name = $"Button - {buildingBase.name}";
        buttonGO.GetComponent<Button>().onClick.AddListener(() => MouseController.Instance.SetMode_Build(buildingBase));
        buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = buildingBase.name;
    }
}
