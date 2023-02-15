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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeTeams();
    }

    private void InitializeTeams()
    {
        for (int i = 0; i < teamOneChips.Count; i++)
        {
            Chip chip = Instantiate(ChipPrefab, teamOneSpawnPositions[i].position, Quaternion.identity);
            chip.InitializeChip(teamOneChips[i]);
        }

        for (int i = 0; i < teamTwoChips.Count; i++)
        {
            Chip chip = Instantiate(ChipPrefab, teamTwoSpawnPositions[i].position, Quaternion.identity);
            chip.InitializeChip(teamTwoChips[i]);
        }
    }
}
