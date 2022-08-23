using Assets._Scripts._3Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonManager : Singleton<SeasonManager>
{
    [SerializeField]
    private int TicksPerBrightSeason;

    [SerializeField]
    private int TicksPerDarkSeason;

    public float SeasonProgressionPercentage { get => CurrentSeasonProgressionPercentage(); }
    public Quaternion[] CurrentSeasonRotationBounds { get => GetCurrentSeasonRotationBounds(); }

    public Season CurrentSeason { get => currentSeason; }
    public Season NextSeason { get => (Season)(((int)CurrentSeason + 1) % 2); }
    private Season currentSeason;
    private Logger logger;

    public static event EventHandler<Season> OnSeasonChange;

    void Start()
    {
        currentSeason = Season.Bright;
        TimeTicker.OnTick += OnTick;
        logger = Logger.Instance;
    }

    private int GetTicksPerCurrentSeason()
    {
        return currentSeason == Season.Bright ? TicksPerBrightSeason : TicksPerDarkSeason;
    }

    private void GoToNextSeason()
    {
        int season = (int)currentSeason;
        currentSeason = NextSeason;
        logger.LogMessage($"Season has changed to {currentSeason} season", Logger.LogType.Debug);
        OnSeasonChange?.Invoke(this, currentSeason);
    }

    private float CurrentSeasonProgressionPercentage()
    {
        return (((float) TimeTicker.GetInnerTick(TimeTicker.START_OF_THE_GAME)) % GetTicksPerCurrentSeason()) / ((float) GetTicksPerCurrentSeason());
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

        return currentSeason == Season.Bright ? new Quaternion[] { Quaternion.Euler(0,0,0), rotationForBrightSeason } 
                                            : new Quaternion[] { rotationForBrightSeason, rotationForDarkSeason };
    }
}

public enum Season
{
    Bright,
    Dark
}