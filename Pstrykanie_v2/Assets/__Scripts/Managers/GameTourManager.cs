using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTourManager : SingletonMonobehaviour<GameTourManager>
{
    public event EventHandler OnStateChanged;
    public event EventHandler OnGameOver;
    public event EventHandler<OnNextStateChangedArgs> OnNextStateChanged;

    public class OnNextStateChangedArgs : EventArgs
    {
        public GameState nextGameState;
    }

    private GameState currentState;
    private GameState previousGameState;
    private GameState nextGameState;

    private float changingTurnVignetteTimer;
    private float changingTurnVignetteTime = 1f;
    private int winningTeamID;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        RollStartingTeam();
    }

    private void Update()
    {
        UpdateTimers();

        switch (currentState)
        {
            case GameState.IsChangingTurn:
                GameManager.Instance.SetVignete(nextGameState, false);
                if (changingTurnVignetteTimer <= 0f)
                {
                    ChangeGameState(nextGameState);
                    nextGameState = GameState.None;
                    GameManager.Instance.SetVignete(nextGameState, true);
                }
                break;
        }

        switch (nextGameState)
        {
            case GameState.TeamOneTurn:
            case GameState.TeamTwoTurn:
                OnNextStateChanged(this, new OnNextStateChangedArgs { nextGameState = nextGameState });
                break;
            default:
                OnNextStateChanged(this, new OnNextStateChangedArgs { nextGameState = GameState.None });
                break;
        }
    }

    private void UpdateTimers()
    {
        if (changingTurnVignetteTimer > 0)
        {
            changingTurnVignetteTimer -= Time.deltaTime;
        }
    }

    private void ChangingTurn()
    {
        currentState = GameState.IsChangingTurn;
        changingTurnVignetteTimer = changingTurnVignetteTime;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void RollStartingTeam()
    {
        int startingTeam = UnityEngine.Random.Range(1, 3);
        if (startingTeam == 1)
        {
            nextGameState = GameState.TeamOneTurn;
            ChangingTurn();
        }
        else
        {
            nextGameState = GameState.TeamTwoTurn;
            ChangingTurn();
        }
    }

    public void ChangeGameState(int chipID, bool wait = false)
    {
        if (chipID == 1 && !wait && GameManager.Instance.CheckIfAnyTeamHasChip())
        {
            nextGameState = GameState.TeamTwoTurn;
            ChangingTurn();
        }
        else if (chipID == 2 && !wait && GameManager.Instance.CheckIfAnyTeamHasChip())
        {
            nextGameState = GameState.TeamOneTurn;
            ChangingTurn();
        }
        else
        {
            currentState = GameState.Waiting;
            if (chipID == 1)
            {
                previousGameState = GameState.TeamOneTurn;
            }
            else
            {
                previousGameState = GameState.TeamTwoTurn;
            }
        }

        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeGameState(GameState gameState)
    {
        currentState = gameState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void CheckForGameOver()
    {
        if (!GameManager.Instance.CheckIfTeamHasChip(1))
        {
            winningTeamID = 2;
            GameOver();
        }

        if (!GameManager.Instance.CheckIfTeamHasChip(2))
        {
            winningTeamID = 1;
            GameOver();
        }
    }

    private void GameOver()
    {
        currentState = GameState.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public GameState GetCurrentGameState()
    {
        return currentState;
    }

    public GameState GetNextGameState()
    {
        return nextGameState;
    }

    public int GetWinningTeamID()
    {
        return winningTeamID;
    }
}
