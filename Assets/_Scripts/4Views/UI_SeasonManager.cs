using UnityEngine;

public class UI_SeasonManager : MonoBehaviour
{
    private RectTransform _trans;
    private float _percentageToNextSeason;

    void Start()
    {
        _trans = GetComponent<RectTransform>();
        SeasonManager.OnSeasonChange += OnSeasonChange;
    }

    private void Update()
    {
        _percentageToNextSeason = SeasonManager.Instance.SeasonProgressionPercentage;
        var bounds = SeasonManager.Instance.CurrentSeasonRotationBounds;
        _trans.rotation = Quaternion.Slerp(bounds[0], bounds[1], _percentageToNextSeason);
    }

    private void OnSeasonChange(object obj, Season newSeason)
    {

    }
}
