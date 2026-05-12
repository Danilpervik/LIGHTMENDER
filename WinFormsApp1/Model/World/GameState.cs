using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Model.World;

public class GameState
{
    public GameStateEnum CurrentState { get; set; }
    public int CurrentLevelIndex { get; set; }
    public int TotalLevels { get; set; }
    public enum GameStateEnum
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public GameState()
    {
        CurrentState = GameStateEnum.MainMenu;
        CurrentLevelIndex = 0;
        TotalLevels = 3;
    }
    public bool IsPlaying()
    {
        return CurrentState == GameStateEnum.Playing;
    }

    public bool IsPaused()
    {
        return CurrentState == GameStateEnum.Paused;
    }

    public bool IsGameOver()
    {
        return CurrentState == GameStateEnum.GameOver;
    }

    public bool IsVictory()
    {
        return CurrentState == GameStateEnum.Victory;
    }

    public bool IsInMenu()
    {
        return CurrentState == GameStateEnum.MainMenu;
    }

    public void StartGame()
    {
        CurrentState = GameStateEnum.Playing;
        CurrentLevelIndex = 0;
    }
    public void PauseGame()
    {
        CurrentState = GameStateEnum.Paused;
    }
    public void ResumeGame()
    {
        CurrentState = GameStateEnum.Playing;
    }
    public void GameOver()
    {
        CurrentState = GameStateEnum.GameOver;
    }
    public void Victory()
    {
        CurrentState = GameStateEnum.Victory;
    }
    public void LevelCompleted()
    {
        if (CurrentLevelIndex < TotalLevels - 1)
        {
            CurrentLevelIndex++;
            CurrentState = GameStateEnum.Playing;
        }
        else
            Victory();
    }
    public void BackToMainMenu()
    {
        CurrentState = GameStateEnum.MainMenu;
        CurrentLevelIndex = 0;
    }
}
