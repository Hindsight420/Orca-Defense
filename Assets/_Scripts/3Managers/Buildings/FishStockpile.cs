using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStockpile : Stockpile<Fish>
{
    private void Awake()
    {
        ConfigureStore(15);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddToStore();
        } 
    }

}
