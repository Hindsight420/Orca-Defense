using Assets._Scripts._3Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI CurrentTick;

    // Update is called once per frame
    void Update()
    {
        CurrentTick.text = TimeTicker.CurrentTick.ToString();
    }
}
