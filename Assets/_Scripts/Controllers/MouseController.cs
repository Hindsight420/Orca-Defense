using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : Singleton<MouseController>
{
    IslandController islandController;

    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    bool buildOrDestroy;

    GameObject selectedBuilding;
    BuildingBase buildingBase;

    // Start is called before the first frame update
    void Start()
    {
        islandController = IslandController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        UpdateCameraMovement();
        UpdateDraggingPreview();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateDraggingPreview()
    {
        if (selectedBuilding == null) return; // No building currently selected
        if (EventSystem.current.IsPointerOverGameObject()) return; // Mouse is over UI element
        if (Input.GetMouseButtonDown(1))
        {
            // Cancel current build
            DestroyPreview();
            return;
        };

        Tile selectedTile = islandController.Island.GetTileAtCoords(currFramePosition);
        if (selectedTile == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (buildOrDestroy)
                islandController.Island.TryPlaceBuilding(selectedTile, buildingBase);
            else
                islandController.Island.TryDestroyBuilding(selectedTile);

            DestroyPreview();
        }
        else
        {
            selectedBuilding.transform.position = new Vector3(selectedTile.X, selectedTile.Y, 0);
        }
    }

    void DestroyPreview()
    {
        Destroy(selectedBuilding);
        selectedBuilding = null;
    }

    void UpdateCameraMovement()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }

    public void SetMode_Build(BuildingBase buildingBase)
    {
        this.buildingBase = buildingBase;
        buildOrDestroy = true;
        Destroy(selectedBuilding);
        selectedBuilding = Instantiate(buildingBase.Prefab);
        SpriteRenderer sr = selectedBuilding.GetComponent<SpriteRenderer>();
        sr.color = Color.gray;
    }

    public void SetMode_Destroy()
    {
        buildOrDestroy = false;
        Destroy(selectedBuilding);
        selectedBuilding = Instantiate(DataSystem.Instance.BuildingBases.Single(b => b.name == "Square").Prefab);
        SpriteRenderer sr = selectedBuilding.GetComponent<SpriteRenderer>();
        sr.color = Color.red;
    }
}
