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
    public Season CurrentSeason { get => currentSeason; }
    private Season currentSeason;
    private Logger logger;

    public static event EventHandler<Season> OnSeasonChange;

    void Start()
    {
        currentSeason = Season.Bright;
        TimeTicker.OnTick += OnTick;
        logger = Logger.Instance;
    }

    private int GetTicksPerCurrentSeason ()
    {
        return currentSeason == Season.Bright ? TicksPerBrightSeason : TicksPerDarkSeason;
    }

    private void NextSeason ()
    {
        int season = (int)currentSeason;
        currentSeason = (Season)((season + 1) % 2);
        logger.LogMessage($"Season has changed to {currentSeason} season", Logger.LogType.Debug);
        OnSeasonChange?.Invoke(this, currentSeason);
    }

    private float CurrentSeasonProgressionPercentage()
    {
        return TimeTicker.GetInnerTick(TimeTicker.START_OF_THE_GAME) / GetTicksPerCurrentSeason();
    }

    private void OnTick(object obj, int tick)
    {
        if (TimeTicker.GetInnerTick(TimeTicker.START_OF_THE_GAME) % GetTicksPerCurrentSeason() == 0)
        {
            NextSeason();
        }
    }
}

public enum Season
{
    Bright,
    Dark
}