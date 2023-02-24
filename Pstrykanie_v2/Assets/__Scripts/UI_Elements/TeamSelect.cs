    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamSelect : MonoBehaviour
{
    [SerializeField] private int teamNumber;
    [SerializeField] private List<ChipTeamSlot> slots =  new List<ChipTeamSlot>();

    private void Start()
    {
        SelectTeamsUI.Instance.OnSelectTeamConfirm += SelectTeamsUI_OnSelectTeamConfirm;
    }

    private void SelectTeamsUI_OnSelectTeamConfirm(object sender, System.EventArgs e)
    {
        AddTeams();
    }

    private void AddTeams()
    {
        List<ChipSO> teamMembers = new List<ChipSO>();
        foreach (ChipTeamSlot chip in slots)
        {
            teamMembers.Add(chip.GetChipInTeamSlot());
        }

        GameSettings.Instance.SetTeamChips(teamMembers, teamNumber);
    }
}
