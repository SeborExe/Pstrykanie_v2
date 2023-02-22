using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public enum GameState
    {
        TeamOneTurn,
        TeamTwoTurn,
        Waiting,
        IsChangingTurn,
        GameOver
    }

    public event EventHandler OnStateChanged;

    private GameState currentState;
    private GameState previousGameState;

    [Header("Chips")]
    [SerializeField] List<ChipSO> teamOneChips = new List<ChipSO>();
    [SerializeField] List<ChipSO> teamTwoChips = new List<ChipSO>();
    [SerializeField] List<Transform> teamOneSpawnPositions = new List<Transform>();
    [SerializeField] List<Transform> teamTwoSpawnPositions = new List<Transform>();
    [SerializeField] private Chip ChipPrefab;
    [SerializeField] List<Chip> AllChips = new List<Chip>();

    [Header("Components")]
    [SerializeField] private Volume volume;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private int chipTeamOneCount;
    private int chipTeamTwoCount;

    private Vignette vignette;
    private GameState nextTeamTurn;
    private float changingTurnVignetteTimer;
    private float changingTurnVignetteTime = 3f;

    protected override void Awake()
    {
        base.Awake();

        RollStartingTeam();
        volume.profile.TryGet<Vignette >(out vignette);
    }

    private void Start()
    {
        InitializeTeams();

        DeadZone.Instance.OnChipFall += DeadZone_OnChipFall;
    }

    private void Update()
    {
        UpdateTimers();

        switch (currentState)
        {
            case GameState.IsChangingTurn:
                SetVignete(nextTeamTurn, false);
                if (changingTurnVignetteTimer <= 0f)
                {
                    currentState = nextTeamTurn;
                    SetVignete(nextTeamTurn, true);
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
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
            nextTeamTurn = GameState.TeamOneTurn;
            currentState = GameState.IsChangingTurn;
            changingTurnVignetteTimer = changingTurnVignetteTime;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            nextTeamTurn = GameState.TeamTwoTurn;
            currentState = GameState.IsChangingTurn;
            changingTurnVignetteTimer = changingTurnVignetteTime;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
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
            nextTeamTurn = GameState.TeamTwoTurn;
            currentState = GameState.IsChangingTurn;
            previousGameState = GameState.Waiting;
            changingTurnVignetteTimer = changingTurnVignetteTime;
        }
        else if (chipID == 2 && !wait && (chipTeamTwoCount != 0 && chipTeamOneCount != 0))
        {
            nextTeamTurn = GameState.TeamOneTurn;
            currentState = GameState.IsChangingTurn;
            previousGameState = GameState.Waiting;
            changingTurnVignetteTimer = changingTurnVignetteTime;
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

    private void SetVignete(GameState nextTeamTurn, bool showLowVisibleViggnete)
    {
        if (!showLowVisibleViggnete)
        {
            vignette.intensity.value = 0.35f;
        }
        else
        {
            vignette.intensity.value = 0.25f;
        }

        if (nextTeamTurn == GameState.TeamOneTurn)
        {
            vignette.color.value = Color.blue;
        }
        else if (nextTeamTurn == GameState.TeamTwoTurn)
        {
            vignette.color.value = Color.red;
        }
    }
}
