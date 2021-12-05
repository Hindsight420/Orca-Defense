using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    Vector3 lastFramePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(1))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
    }
}
