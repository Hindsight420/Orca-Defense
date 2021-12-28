using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    IslandController islandController;

    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    bool buildOrDestroy;

    GameObject selectedBuilding;

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
            UpdateBuildingPreview();
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateBuildingPreview()
    {
        // If we're over a UI element, then bail out from this.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector3 selectedPosition = (buildOrDestroy) ? islandController.Island.GetHighestPositionAtCoords(currFramePosition) : islandController.Island.GetPositionAtCoords(currFramePosition);
        if (selectedPosition.x == -1) return;



        if (Input.GetMouseButtonDown(0))
        {
            if (buildOrDestroy)
            {
                islandController.Island.PlaceBuilding((int)selectedPosition.x, (int)selectedPosition.y);
            }
            else
            {
                // add logic for destroying buildings
                Debug.Log("Trying to destroy building");
                //islandController.Island.RemoveBuilding();


                // add dragging logic?
            }
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
        buildOrDestroy = true;
        Destroy(selectedBuilding);
        selectedBuilding = new GameObject();
        selectedBuilding.transform.position = islandController.Island.GetHighestPositionAtCoords(currFramePosition);
        SpriteRenderer sr = selectedBuilding.AddComponent<SpriteRenderer>();
        sr.sprite = islandController.squareSprite;
        sr.color = Color.gray;
    }

    public void SetMode_Destroy()
    {
        buildOrDestroy = false;
        Destroy(selectedBuilding);
        selectedBuilding = new GameObject();
        selectedBuilding.transform.position = islandController.Island.GetHighestPositionAtCoords(currFramePosition);
        SpriteRenderer sr = selectedBuilding.AddComponent<SpriteRenderer>();
        sr.sprite = islandController.squareSprite;
        sr.color = Color.red;
    }
}
