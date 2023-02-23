using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [Header("Chips")]
    List<ChipSO> teamOneChips = new List<ChipSO>();
    List<ChipSO> teamTwoChips = new List<ChipSO>();
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

    protected override void Awake()
    {
        base.Awake();

        volume.profile.TryGet<Vignette >(out vignette);
    }

    private void Start()
    {
        GetTeams();
        
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

        GameTourManager.Instance.CheckForGameOver();
    }

    private void GetTeams()
    {
        teamOneChips = GameSettings.Instance.GetTeamChips(1);
        teamTwoChips = GameSettings.Instance.GetTeamChips(2);

        InitializeTeams();
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

    public int GetCurrentGameStateTeamNumber()
    {
        if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamOneTurn) return 1;

        if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamTwoTurn) return 2;

        else return 0;
    }

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        CinemachineShake cinemachineShake = virtualCamera.GetComponent<CinemachineShake>();
        cinemachineShake.ShakeCamera(amplitude, frequency, time);
    }

    public void SetVignete(GameState nextTeamTurn, bool showLowVisibleViggnete)
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

    public bool CheckIfAnyTeamHasChip()
    {
        return chipTeamTwoCount != 0 && chipTeamOneCount != 0;
    }

    public bool CheckIfTeamHasChip(int teamNumber)
    {
        if (teamNumber == 1) return chipTeamOneCount != 0;
        if (teamNumber == 2) return chipTeamTwoCount != 0;
        else return false;
    }
}
