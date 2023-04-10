using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PenguinFactory : MonoBehaviour
{
    [SerializeField]
    GameObject PenguinPrefab;

    [SerializeField]
    List<Sprite> BeakTops;
    [SerializeField]
    List<Sprite> BeakBottoms;

    [SerializeField]
    List<Sprite> Wings;

    [SerializeField]
    List<Sprite> Head;

    [SerializeField]
    List<Sprite> Body;

    private static float PenguinZOffset = 0;

    Vector3 Position => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GeneratePenguin(Position);
        }
    }

    void GeneratePenguin (Vector2 position)
    {
        var pen = Instantiate(PenguinPrefab);
        pen.transform.position = new Vector3(position.x, position.y, PenguinZOffset);

        var penScript = pen.GetComponent<Penguin>();
        penScript.BeakBottom.sprite = BeakBottoms[Random.Range(0, BeakBottoms.Count)];
        penScript.BeakTop.sprite = BeakTops[Random.Range(0, BeakTops.Count)];
        penScript.Body.sprite = Body[Random.Range(0, Body.Count)];
        penScript.Head.sprite = Head[Random.Range(0, Head.Count)];

        //Same wings
        var wings = Wings[Random.Range(0, Wings.Count)];
        penScript.WingLeft.sprite = wings;
        penScript.WingRight.sprite = wings;


        PenguinZOffset += 0.5f;
    }
}
