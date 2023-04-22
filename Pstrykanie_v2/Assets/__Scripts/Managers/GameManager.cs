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
    [SerializeField] private Chip ChipPrefab;
    List<ChipSO> teamOneChips = new List<ChipSO>();
    List<ChipSO> teamTwoChips = new List<ChipSO>();

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private int chipTeamOneCount;
    private int chipTeamTwoCount;

    public int TeamOneChipToPlaceRemains { get; private set; }
    public int TeamTwoChipToPlaceRemains { get; private set; }

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

        TeamOneChipToPlaceRemains = teamOneChips.Count;
        TeamTwoChipToPlaceRemains = teamTwoChips.Count;

        gameTourManager.RollStartingTeam();
    }

    public void InitializeChip(Vector3 position, int team)
    {
        Chip chip = Instantiate(ChipPrefab, position, Quaternion.identity);

        if (team == 1)
            chip.InitializeChip(teamOneChips[TeamOneChipToPlaceRemains - 1], team);
        else
            chip.InitializeChip(teamTwoChips[TeamTwoChipToPlaceRemains - 1], team);
    }

    public ChipSO GetNextChipToPlace(int team)
    {
        if (team == 1)
            return teamOneChips[TeamOneChipToPlaceRemains - 1];
        else
            return teamTwoChips[TeamTwoChipToPlaceRemains - 1];
    }

    public void SetTeamCount()
    {
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

    public void DecreaseRemainingsChipToPlace(int team)
    {
        if (team == 1)
            TeamOneChipToPlaceRemains--;
        else
            TeamTwoChipToPlaceRemains--;
    }
}
