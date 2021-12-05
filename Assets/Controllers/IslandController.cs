using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    Island island;
    public Sprite tileSprite;

    // Start is called before the first frame update
    void Start()
    {
        island = new Island();

        for (int x = 0; x < island.Width; x++)
        {
            for (int y = 0; y < island.Height; y++)
            {
                GameObject tile_go = new GameObject();
                tile_go.name = $"Tile_{x}_{y}";
                tile_go.transform.position = new Vector3(x, y, 0);

                if (y == 0)
                {
                    SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer>();
                    tile_sr.sprite = tileSprite;
                }
            }
        }
    }

    void Update()
    {

    }
}
