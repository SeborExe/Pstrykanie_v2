using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamObject : MonoBehaviour
{
    private Team team;

    [SerializeField] private Button team_1_Button;
    [SerializeField] private Button team_2_Button;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Image chipOneImage;
    [SerializeField] private Image chipTwoImage;
    [SerializeField] private Image chipThreeImage;
    [SerializeField] private TMP_Text teamName;

    private void OnEnable()
    {
        team_1_Button.onClick.AddListener(() => SelectTeamsUI.Instance.LoadTeamFromSaved(1, team.TeamMembers));
        team_2_Button.onClick.AddListener(() => SelectTeamsUI.Instance.LoadTeamFromSaved(2, team.TeamMembers));
        deleteButton.onClick.AddListener(() =>
        {
            DeleteTeamPopUp deleteTeamPopUp = Instantiate(SelectTeamsUI.Instance.deleteTeamPopUp, 
                SelectTeamsUI.Instance.transform.position + new Vector3(0, -700, 0), Quaternion.identity);
            deleteTeamPopUp.transform.SetParent(SelectTeamsUI.Instance.transform);
            deleteTeamPopUp.Initialize(team);
            deleteTeamPopUp.transform.localScale = Vector3.one;
        });
    }

    private void OnDisable()
    {
        team_1_Button.onClick.RemoveAllListeners();
        team_2_Button.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
    }

    public void SetTeam(Team team)
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

        teamName.text = team.teamName;
    }
}
