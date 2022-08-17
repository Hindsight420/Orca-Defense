using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SeasonManager : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        SeasonManager.OnSeasonChange += OnSeasonChange;
    }

    private void OnSeasonChange(object obj, Season newSeason)
    {
        text.text = $"{newSeason} Season";
    }
}
