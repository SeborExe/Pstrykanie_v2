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

    protected override void Awake()
    {
        base.Awake();

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

    private void Start()
    {
        InitializeTeams();

        DeadZone.Instance.OnChipFall += DeadZone_OnChipFall;
    }

    private void Update()
    {
        Debug.Log(currentState);
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

    private void CheckForGameOver()
    {
        if (chipTeamOneCount == 0)
        {
            //Team Two WIN!
            Debug.Log("Team TWO WIN!");
        }

        if (chipTeamTwoCount == 0)
        {
            //Team One WIN!
            Debug.Log("Team ONE WIN!");
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

    public void SetCurrentGameState(GameState gameState)
    {
        currentState = gameState;
    }

    public void SetPreviousGameState(GameState gameState)
    {
        previousGameState = gameState;
    }

    public void ChangeGameState(int chipID)
    {
        if (chipID == 1)
        {
            currentState = GameState.TeamTwoTurn;
        }
        else
        {
            currentState = GameState.TeamOneTurn;
        }
    }
}
