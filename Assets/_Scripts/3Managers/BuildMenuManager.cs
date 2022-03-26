using UnityEngine;

public class BuildMenuManager : MonoBehaviour
{
    public BuildMenuView View;

    void Start()
    {
        View.InitializeButtons(OnBuildingButtonClicked);
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
