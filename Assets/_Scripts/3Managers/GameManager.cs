using EventCallbacks;
using System;

public class GameManager : Singleton<GameManager>
{
    public GameState State;

    public BuildingType SelectedBuilding;

    void Start()
    {
        UpdateGameState(GameState.Play);
    }

    public void UpdateGameState(GameState state)
    {
        State = state;

        switch (state)
        {
            case GameState.Pause:
                HandlePause();
                break;
            case GameState.Play:
                HandlePlay();
                break;
            case GameState.Build:
                HandleBuild();
                break;
            case GameState.Destroy:
                HandleDestroy();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        new GameStateChangedEvent() { State = state }.FireEvent();
    }

    private void HandlePause()
    {
        throw new NotImplementedException();
    }

    private void HandlePlay()
    {
        //throw new NotImplementedException();
    }

    private void HandleBuild()
    {
        //throw new NotImplementedException();
    }

    private void HandleDestroy()
    {
        //throw new NotImplementedException();
    }
}

public class GameStateChangedEvent : Event<GameStateChangedEvent>
{
    public GameState State;
}

public enum GameState
{
    Pause,
    Play,
    Build,
    Destroy
}