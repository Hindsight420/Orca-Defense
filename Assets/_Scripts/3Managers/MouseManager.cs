using OrcaDefense.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    private IslandManager _islandManager;
    private Vector3 _lastFramePosition;
    private Vector3 _currFramePosition;
    private GameObject _buildingPreview;
    private BuildingType _buildingType;
    private GameState _state;

    void Start()
    {
        _islandManager = IslandManager.Instance;
        _state = GameManager.Instance.State;

        GameStateChangedEvent.RegisterListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent gameStateEvent)
    {
        _state = gameStateEvent.State;
        _buildingType = GameManager.Instance.SelectedBuilding;

        if (_state == GameState.Build || _state == GameState.Destroy)
        {
            CreatePreview();
        }
    }

    private void CreatePreview()
    {
        Destroy(_buildingPreview);
        _buildingPreview = Instantiate(_buildingType.PreviewPrefab, transform);

        SpriteRenderer sr = _buildingPreview.GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, .5f);
    }

    void Update()
    {
        _currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currFramePosition.z = 0;

        UpdateCameraMovement();
        UpdateBuild();

        _lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _lastFramePosition.z = 0;
    }

    void UpdateCameraMovement()
    {
        if (_state != GameState.Play) return;

        if (Input.GetMouseButton(1))
        {
            Vector3 diff = _lastFramePosition - _currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }

    void UpdateBuild()
    {
        if (_state != GameState.Build && _state != GameState.Destroy) return;

        // No building currently selected
        if (_buildingPreview == null) return;

        // Right click to cancel build
        if (Input.GetMouseButtonDown(1))
        {
            CancelBuild();
            return;
        };

        // Mouse is over UI element
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Tile selectedTile = _islandManager.Island.GetTileAtCoords(_currFramePosition);
        if (selectedTile == null) return;

        // Left click to build
        if (Input.GetMouseButtonDown(0))
        {
            if (_state == GameState.Build)
                _islandManager.TryPlaceBuilding(selectedTile, _buildingType);
            if (_state == GameState.Destroy)
                _islandManager.TryDestroyBuilding(selectedTile);

            // Hold shift to remain in build mode
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                DestroyPreview();
                GameManager.Instance.UpdateGameState(GameState.Play);
            }
        }
        else
        {
            _buildingPreview.transform.position = new Vector3(selectedTile.X, selectedTile.Y, 0);
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
        Destroy(_buildingPreview);
        _buildingPreview = null;
    }
}
