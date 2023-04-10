using OrcaDefense.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    IslandManager islandManager;

    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    GameObject buildingPreview;
    BuildingType buildingType;

    GameState state;

    void Start()
    {
        islandManager = IslandManager.Instance;
        state = GameManager.Instance.State;

        GameStateChangedEvent.RegisterListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent gameStateEvent)
    {
        state = gameStateEvent.State;
        buildingType = GameManager.Instance.SelectedBuilding;

        if (state == GameState.Build || state == GameState.Destroy)
        {
            CreatePreview();
        }
    }

    private void CreatePreview()
    {
        Destroy(buildingPreview);
        buildingPreview = Instantiate(buildingType.PreviewPrefab, transform);

        SpriteRenderer sr = buildingPreview.GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, .5f);
    }

    void Update()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        UpdateCameraMovement();
        UpdateBuild();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateCameraMovement()
    {
        if (state != GameState.Play) return;

        if (Input.GetMouseButton(1))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }

    void UpdateBuild()
    {
        if (state != GameState.Build && state != GameState.Destroy) return;

        // No building currently selected
        if (buildingPreview == null) return;

        // Right click to cancel build
        if (Input.GetMouseButtonDown(1))
        {
            CancelBuild();
            return;
        };

        // Mouse is over UI element
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Tile selectedTile = islandManager.Island.GetTileAtCoords(currFramePosition);
        if (selectedTile == null) return;

        // Left click to build
        if (Input.GetMouseButtonDown(0))
        {
            if (state == GameState.Build)
                islandManager.TryPlaceBuilding(selectedTile, buildingType);
            if (state == GameState.Destroy)
                islandManager.TryDestroyBuilding(selectedTile);

            // Hold shift to remain in build mode
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                DestroyPreview();
                GameManager.Instance.UpdateGameState(GameState.Play);
            }
        }
        else
        {
            buildingPreview.transform.position = new Vector3(selectedTile.X, selectedTile.Y, 0);
        }
    }

    void CancelBuild()
    {
        GameManager.Instance.UpdateGameState(GameState.Play);
        DestroyPreview();
        return;
    }

    void DestroyPreview()
    {
        Destroy(buildingPreview);
        buildingPreview = null;
    }
}
