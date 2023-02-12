using Assets._Scripts._3Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentTick;

    // Update is called once per frame
    void Update()
    {
        _currentTick.text = TimeTicker.CurrentTick.ToString();
    }
}
