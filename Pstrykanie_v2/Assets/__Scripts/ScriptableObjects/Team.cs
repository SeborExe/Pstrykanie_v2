using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team
{
    public string teamName;
    public List<ChipSO> TeamMembers = new List<ChipSO>();

    public Team(List<ChipSO> teamMembers, string teamName)
    {
        TeamMembers = teamMembers;
        this.teamName = teamName;
    }
}
