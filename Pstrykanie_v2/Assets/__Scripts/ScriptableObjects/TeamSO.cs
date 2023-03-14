using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSO
{
    public List<ChipSO> TeamMembers = new List<ChipSO>();

    public TeamSO(List<ChipSO> teamMembers)
    {
        TeamMembers = teamMembers;
    }
}
