using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : SingletonMonobehaviour<GameSettings>
{
    public event EventHandler OnTeamMemberChanged;

    [SerializeField] List<ChipSO> teamOneChips = new List<ChipSO>();
    [SerializeField] List<ChipSO> teamTwoChips = new List<ChipSO>();
    [SerializeField] List<MapSO> allMaps = new List<MapSO>();

    private MapSO selectedMap;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        SetSelectedMap(allMaps[0]);
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
