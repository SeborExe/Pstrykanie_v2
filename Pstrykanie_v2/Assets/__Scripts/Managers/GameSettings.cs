using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : SingletonMonobehaviour<GameSettings>
{
    public event EventHandler OnTeamMemberChanged;

    [SerializeField] List<MapSO> allMaps = new List<MapSO>();

    private List<ChipSO> teamOneChips = new List<ChipSO>();
    private List<ChipSO> teamTwoChips = new List<ChipSO>();
    private MapSO selectedMap;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        SetSelectedMap(allMaps[0]);
    }

    public void SetTeamChips(List<ChipSO> chips, int teamNumber)
    {
        if (teamNumber == 1)
        {
            teamOneChips.Clear();
            foreach (ChipSO chip in chips) 
            { 
                teamOneChips.Add(chip);
            }
        }

        else if (teamNumber == 2)
        {
            teamTwoChips.Clear();
            foreach (ChipSO chip in chips)
            {
                teamTwoChips.Add(chip);
            }
        }
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

    public bool TeamsHasEnoughMembers()
    {
        return teamOneChips.Count >= 3 && teamTwoChips.Count >= 3;
    }

    public bool TeamHasEnoughMembers(int teamID)
    {
        if (teamID == 1)
        {
            return teamOneChips.Count >= 3;
        }
        else if (teamID == 2)
        {
            return teamTwoChips.Count >= 3;
        }

        else return false;
    }

    public List<MapSO> GetAllMaps()
    {
        return allMaps;
    }

    public MapSO GetCurrentSelectedMap()
    {
        return selectedMap;
    }

    public void SetSelectedMap(MapSO map)
    {
        selectedMap = map;
    }
}
