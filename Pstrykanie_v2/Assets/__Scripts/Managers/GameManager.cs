using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [SerializeField] List<ChipSO> teamOneChips = new List<ChipSO>();
    [SerializeField] List<ChipSO> teamTwoChips = new List<ChipSO>();
    [SerializeField] List<Transform> teamOneSpawnPositions = new List<Transform>();
    [SerializeField] List<Transform> teamTwoSpawnPositions = new List<Transform>();
    [SerializeField] private Chip ChipPrefab;

    private int chipTeamOneCount;
    private int chipTeamTwoCount;

    protected override void Awake()
    {
        base.Awake();
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
        }

        for (int i = 0; i < teamTwoChips.Count; i++)
        {
            Chip chip = Instantiate(ChipPrefab, teamTwoSpawnPositions[i].position, Quaternion.identity);
            chip.InitializeChip(teamTwoChips[i], 2);
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
}
