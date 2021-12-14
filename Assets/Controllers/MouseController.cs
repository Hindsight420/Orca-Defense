using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    IslandController islandController;

    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    GameObject selectedBuilding;
    public GameObject squarePrefab;

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

        if(selectedBuilding != null)
        {
            UpdateBuildingPreview();
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateBuildingPreview()
    {
        Vector3 selectedPosition = islandController.Island.GetHighestPositionAtCoords(currFramePosition);
        if (selectedPosition.x == -1) return;

        Debug.Log(islandController);
        if (Input.GetMouseButtonUp(0))
        {
            islandController.Island.PlaceBuilding(selectedPosition);
            Destroy(selectedBuilding);
            selectedBuilding = null;
        }
        else
        {
            selectedBuilding.transform.position = selectedPosition;
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

    public void SetMode_Build()
    {
        Destroy(selectedBuilding);
        selectedBuilding = Instantiate(squarePrefab);
    }
}
