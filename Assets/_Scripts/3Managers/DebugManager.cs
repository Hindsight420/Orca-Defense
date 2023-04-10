using Assets._Scripts._3Managers;
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
