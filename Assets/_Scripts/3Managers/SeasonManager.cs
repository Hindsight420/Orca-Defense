using Assets._Scripts._3Managers;
using System;
using UnityEngine;

public class SeasonManager : Singleton<SeasonManager>
{
    [SerializeField]
    private int _ticksPerBrightSeason;
    [SerializeField]
    private int _ticksPerDarkSeason;
    private Logger _logger;

    public float SeasonProgressionPercentage { get => CurrentSeasonProgressionPercentage(); }
    public Quaternion[] CurrentSeasonRotationBounds { get => GetCurrentSeasonRotationBounds(); }
    public Season CurrentSeason { get; private set; }
    public Season NextSeason { get => (Season)(((int)CurrentSeason + 1) % 2); }

    public static event EventHandler<Season> OnSeasonChange;

    void Start()
    {
        CurrentSeason = Season.Bright;
        TimeTicker.OnTick += OnTick;
        _logger = Logger.Instance;
    }

    private int GetTicksPerCurrentSeason()
    {
        return CurrentSeason == Season.Bright ? _ticksPerBrightSeason : _ticksPerDarkSeason;
    }

    private void GoToNextSeason()
    {
        int season = (int)CurrentSeason;
        CurrentSeason = NextSeason;
        _logger.LogMessage($"Season has changed to {CurrentSeason} season", Logger.LogType.Debug);
        OnSeasonChange?.Invoke(this, CurrentSeason);
    }

    private float CurrentSeasonProgressionPercentage()
    {
        return (((float)TimeTicker.GetInnerTick(TimeTicker.START_OF_THE_GAME)) % GetTicksPerCurrentSeason()) / ((float)GetTicksPerCurrentSeason());
    }

    private void OnTick(object obj, int tick)
    {
        if (TimeTicker.GetInnerTick(TimeTicker.START_OF_THE_GAME) % GetTicksPerCurrentSeason() == 0)
        {
            GoToNextSeason();
        }
    }

    private Quaternion[] GetCurrentSeasonRotationBounds()
    {
        var rotationForBrightSeason = Quaternion.Euler(0, 0, 180f);
        var rotationForDarkSeason = Quaternion.Euler(0, 0, 360f);

        return CurrentSeason == Season.Bright ? new Quaternion[] { Quaternion.Euler(0, 0, 0), rotationForBrightSeason }
                                            : new Quaternion[] { rotationForBrightSeason, rotationForDarkSeason };
    }
}

public enum Season
{
    Bright,
    Dark
}