using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team
{
    public List<ChipSO> TeamMembers = new List<ChipSO>();

    public Team(List<ChipSO> teamMembers)
    {
        TeamMembers = teamMembers;
    }
}
