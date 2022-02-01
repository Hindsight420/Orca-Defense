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

        if (selectedBuilding != null)
        {
            // If we're over a UI element, then bail out from this.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            UpdateDraggingPreview();
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateDraggingPreview()
    {
        Tile selectedTile = islandController.Island.GetTileAtCoords(currFramePosition);
        if (selectedTile == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (buildOrDestroy)
            {
                islandController.Island.TryPlaceBuilding(selectedTile, buildingBase);
            }
            else
            {
                islandController.Island.TryDestroyBuilding(selectedTile);
            }
            Destroy(selectedBuilding);
            selectedBuilding = null;
        }
        else
        {
            selectedBuilding.transform.position = new Vector3(selectedTile.X, selectedTile.Y, 0);
        }
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
        selectedBuilding = new GameObject();
        SpriteRenderer sr = selectedBuilding.AddComponent<SpriteRenderer>();
        sr.sprite = buildingBase.Sprite;
        sr.color = Color.gray;
    }

    public void SetMode_Destroy()
    {
        buildOrDestroy = false;
        Destroy(selectedBuilding);
        selectedBuilding = new GameObject();
        SpriteRenderer sr = selectedBuilding.AddComponent<SpriteRenderer>();

        // TODO: Clean up this mess
        sr.sprite = DataSystem.Instance.BuildingBases.Single(b => b.name == "Square").Sprite;
        sr.color = Color.red;
    }
}
