using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    IslandManager islandManager;

    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    GameObject buildingPreview;
    BuildingBase buildingBase;

    GameState state;

    void Start()
    {
        islandManager = IslandManager.Instance;

        GameStateChangedEvent.RegisterListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent gameStateEvent)
    {
        state = gameStateEvent.State;
        buildingBase = GameManager.Instance.SelectedBuilding;

        if (state == GameState.Build || state == GameState.Destroy)
        {
            CreatePreview();
        }
    }

    private void CreatePreview()
    {
        Destroy(buildingPreview);
        buildingPreview = Instantiate(buildingBase.Prefab, transform);

        SpriteRenderer sr = buildingPreview.GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, .5f);
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
        if (!(state == GameState.Build || state == GameState.Destroy))
        {
            return;
        }

        if (buildingPreview == null) return; // No building currently selected
        if (EventSystem.current.IsPointerOverGameObject()) return; // Mouse is over UI element
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.UpdateGameState(GameState.Play);
            // Cancel current build
            DestroyPreview();
            return;
        };

        Tile selectedTile = islandManager.Island.GetTileAtCoords(currFramePosition);
        if (selectedTile == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (state == GameState.Build)
                islandManager.TryPlaceBuilding(selectedTile, buildingBase);
            if (state == GameState.Destroy)
                islandManager.TryDestroyBuilding(selectedTile);

            DestroyPreview();

            GameManager.Instance.UpdateGameState(GameState.Play);
        }
        else
        {
            buildingPreview.transform.position = new Vector3(selectedTile.X, selectedTile.Y, 0);
        }
    }

    void DestroyPreview()
    {
        Destroy(buildingPreview);
        buildingPreview = null;
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
}
