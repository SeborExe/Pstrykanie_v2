using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    GameTourManager gameTourManager;

    [Header("Chips")]
    List<ChipSO> teamOneChips = new List<ChipSO>();
    List<ChipSO> teamTwoChips = new List<ChipSO>();
    [SerializeField] List<Transform> teamOneSpawnPositions = new List<Transform>();
    [SerializeField] List<Transform> teamTwoSpawnPositions = new List<Transform>();
    [SerializeField] private Chip ChipPrefab;

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private int chipTeamOneCount;
    private int chipTeamTwoCount;

    public int ChipsToPlaceRemains { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        gameTourManager = GameTourManager.Instance;

        GetTeams();
        
        DeadZone.Instance.OnChipFall += DeadZone_OnChipFall;
    }

    private void GetTeams()
    {
        teamOneChips = GameSettings.Instance.GetTeamChips(1);
        teamTwoChips = GameSettings.Instance.GetTeamChips(2);

        ChipsToPlaceRemains = teamOneChips.Count + teamTwoChips.Count;

        gameTourManager.RollStartingTeam();
        //InitializeTeams();
    }

    private void InitializeTeams()
    {
        for (int i = 0; i < teamOneChips.Count; i++)
        {
            Chip chip = Instantiate(ChipPrefab, teamOneSpawnPositions[i].position, Quaternion.identity);
            chip.InitializeChip(teamOneChips[i], 1);
        }

        for (int i = 0; i < teamTwoChips.Count; i++)
        {
            Chip chip = Instantiate(ChipPrefab, teamTwoSpawnPositions[i].position, Quaternion.identity);
            chip.InitializeChip(teamTwoChips[i], 2);
        }

        chipTeamOneCount = teamOneChips.Count;
        chipTeamTwoCount = teamTwoChips.Count;
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

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        CinemachineShake cinemachineShake = virtualCamera.GetComponent<CinemachineShake>();
        cinemachineShake.ShakeCamera(amplitude, frequency, time);
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

    public void DecreaseRemainingsChipToPlace()
    {
        ChipsToPlaceRemains--;
    }
}
