using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public enum GameState
    {
        TeamOneTurn,
        TeamTwoTurn,
        Waiting,
        GameOver
    }

    public event EventHandler OnStateChanged;

    private GameState currentState;
    private GameState previousGameState;

    [SerializeField] List<ChipSO> teamOneChips = new List<ChipSO>();
    [SerializeField] List<ChipSO> teamTwoChips = new List<ChipSO>();
    [SerializeField] List<Transform> teamOneSpawnPositions = new List<Transform>();
    [SerializeField] List<Transform> teamTwoSpawnPositions = new List<Transform>();
    [SerializeField] private Chip ChipPrefab;
    [SerializeField] List<Chip> AllChips = new List<Chip>();

    private int chipTeamOneCount;
    private int chipTeamTwoCount;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    protected override void Awake()
    {
        base.Awake();

        RollStartingTeam();
    }

    private void Start()
    {
        InitializeTeams();

        DeadZone.Instance.OnChipFall += DeadZone_OnChipFall;
    }

    private void DeadZone_OnChipFall(object sender, DeadZone.OnChipFallArgs args)
    {
        if (args.chipTeamID == 1)
        {
            chipTeamOneCount--;
        }
        else
        {
            chipTeamTwoCount--;
        }

        CheckForGameOver();
    }

    private void InitializeTeams()
    {
        for (int i = 0; i < teamOneChips.Count; i++)
        {
            Chip chip = Instantiate(ChipPrefab, teamOneSpawnPositions[i].position, Quaternion.identity);
            chip.InitializeChip(teamOneChips[i], 1);
            AllChips.Add(chip);
        }

        for (int i = 0; i < teamTwoChips.Count; i++)
        {
            Chip chip = Instantiate(ChipPrefab, teamTwoSpawnPositions[i].position, Quaternion.identity);
            chip.InitializeChip(teamTwoChips[i], 2);
            AllChips.Add(chip);
        }

        chipTeamOneCount = teamOneChips.Count;
        chipTeamTwoCount = teamTwoChips.Count;
    }

    private void RollStartingTeam()
    {
        int startingTeam = UnityEngine.Random.Range(1, 3);
        if (startingTeam == 1)
        {
            currentState = GameState.TeamOneTurn;
        }
        else
        {
            currentState = GameState.TeamTwoTurn;
        }
    }

    private void CheckForGameOver()
    {
        if (chipTeamOneCount == 0)
        {
            //Team Two WIN!
            Debug.Log("Team TWO WIN!");
            currentState = GameState.GameOver;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }

        if (chipTeamTwoCount == 0)
        {
            //Team One WIN!
            Debug.Log("Team ONE WIN!");
            currentState = GameState.GameOver;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public GameState GetCurrentGameState()
    {
        return currentState;
    }

    public int GetCurrentGameStateTeamNumber()
    {
        if (currentState == GameState.TeamOneTurn) return 1;

        if (currentState == GameState.TeamTwoTurn) return 2;

        else return 0;
    }

    public void ChangeGameState(int chipID, bool wait = false)
    {
        if (chipID == 1 && !wait && (chipTeamTwoCount != 0 && chipTeamOneCount != 0))
        {
            currentState = GameState.TeamTwoTurn;
            previousGameState = GameState.Waiting;
        }
        else if (chipID == 2 && !wait && (chipTeamTwoCount != 0 && chipTeamOneCount != 0))
        {
            currentState = GameState.TeamOneTurn;
            previousGameState = GameState.Waiting;
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

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        CinemachineShake cinemachineShake = virtualCamera.GetComponent<CinemachineShake>();
        cinemachineShake.ShakeCamera(amplitude, frequency, time);
    }
}
