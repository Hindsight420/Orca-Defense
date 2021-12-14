using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    public static IslandController Instance { get; private set; }

    Island island;
    public Island Island { get => island; private set => island = value; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        Island = new Island();
    }

    void Update()
    {

    }
}
