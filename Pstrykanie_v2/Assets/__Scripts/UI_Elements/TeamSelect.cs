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
        AddTeams();
        SelectTeamsUI.Instance.OnSelectTeamConfirm += SelectTeamsUI_OnSelectTeamConfirm;
    }

    private void SelectTeamsUI_OnSelectTeamConfirm(object sender, System.EventArgs e)
    {
        AddTeams();
    }

    private void AddTeams()
    {
        List<ChipSO> teamMembers = new List<ChipSO>();
        teamMembers.Clear();

        foreach (ChipTeamSlot chip in slots)
        {
            if (chip.GetChipInTeamSlot() == null) continue;

            teamMembers.Add(chip.GetChipInTeamSlot());
        }

        GameSettings.Instance.SetTeamChips(teamMembers, teamNumber);
    }

    public List<ChipTeamSlot> GetTeam()
    {
        return slots;
    }

    public void SetTeamFromSaved(List<ChipSO> team)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[0].SetChipData(team[0]);
            slots[1].SetChipData(team[1]);
            slots[2].SetChipData(team[2]);
        }
    }
}
