using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PenguinFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject _penguinPrefab;
    [SerializeField]
    private List<Sprite> _beakTops;
    [SerializeField]
    private List<Sprite> _beakBottoms;
    [SerializeField]
    private List<Sprite> _wings;
    [SerializeField]
    private List<Sprite> _head;
    [SerializeField]
    private List<Sprite> _body;
    private static float PenguinZOffset = 0;
    private Vector3 _position;

    private void Awake()
    {
        _position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GeneratePenguin(_position);
        }
    }

    void GeneratePenguin(Vector2 position)
    {
        var pen = Instantiate(_penguinPrefab);
        pen.transform.position = new Vector3(position.x, position.y, PenguinZOffset);

        var penScript = pen.GetComponent<PenguinBody>();
        penScript.BeakBottom.sprite = _beakBottoms[Random.Range(0, _beakBottoms.Count)];
        penScript.BeakTop.sprite = _beakTops[Random.Range(0, _beakTops.Count)];
        penScript.Body.sprite = _body[Random.Range(0, _body.Count)];
        penScript.Head.sprite = _head[Random.Range(0, _head.Count)];

        //Same wings
        var wings = _wings[Random.Range(0, _wings.Count)];
        penScript.WingLeft.sprite = wings;
        penScript.WingRight.sprite = wings;


        PenguinZOffset += 0.5f;
    }
}
