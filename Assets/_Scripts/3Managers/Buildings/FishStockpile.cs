using UnityEngine;

public class FishStockpile : Stockpile<Fish>
{
    private void Awake()
    {
        ConfigureStore(50);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddToStore();
        } 
    }
}
