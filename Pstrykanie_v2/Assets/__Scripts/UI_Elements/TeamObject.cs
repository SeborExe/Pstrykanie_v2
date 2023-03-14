using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamObject : MonoBehaviour
{
    private TeamSO team;

    [SerializeField] private Button team_1_Button;
    [SerializeField] private Button team_2_Button;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Image chipOneImage;
    [SerializeField] private Image chipTwoImage;
    [SerializeField] private Image chipThreeImage;

    private void OnEnable()
    {
        team_1_Button.onClick.AddListener(() => SelectTeamsUI.Instance.LoadTeamFromSaved(1, team.TeamMembers));
        team_2_Button.onClick.AddListener(() => SelectTeamsUI.Instance.LoadTeamFromSaved(2, team.TeamMembers));
    }

    private void OnDisable()
    {
        team_1_Button.onClick.RemoveAllListeners();
        team_2_Button.onClick.RemoveAllListeners();
    }

    public void SetTeam(TeamSO team)
    {
        this.team = team;
        InitializeTeam();
    }

    private void InitializeTeam()
    {
        List<ChipSO> newTeam = team.TeamMembers;
        for (int i = 0; i < newTeam.Count; i++)
        {
            chipOneImage.sprite = newTeam[0].Image;
            chipTwoImage.sprite = newTeam[1].Image;
            chipThreeImage.sprite = newTeam[2].Image;
        }
    }
}
