using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : SingletonMonobehaviour<TeamManager>
{
    public event EventHandler OnTeamMemberChanged;

    [SerializeField] List<ChipSO> teamOneChips = new List<ChipSO>();
    [SerializeField] List<ChipSO> teamTwoChips = new List<ChipSO>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public List<ChipSO> GetTeamChips(int teamID)
    {
        if (teamID == 1)
        {
            return teamOneChips;
        }

        else if (teamID == 2)
        {
            return teamTwoChips;
        }
        else 
        { 
            return null; 
        }
    }
}
