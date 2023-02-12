using UnityEngine;

public class BuildMenuManager : MonoBehaviour
{
    [SerializeField]
    private BuildMenuView _view;

    void Start()
    {
        _view.InitializeButtons(OnBuildingButtonClicked);
    }

    public void OnBuildingButtonClicked(BuildingType buildingType)
    {
        GameManager.Instance.SelectedBuilding = buildingType;
        GameManager.Instance.UpdateGameState(GameState.Build);
    }

    public void OnDestroyButtonClicked()
    {
        // TODO: Clean up this string reference
        GameManager.Instance.SelectedBuilding = DataSystem.Instance.GetBuildingType("Destroy");
        GameManager.Instance.UpdateGameState(GameState.Destroy);
    }
}
