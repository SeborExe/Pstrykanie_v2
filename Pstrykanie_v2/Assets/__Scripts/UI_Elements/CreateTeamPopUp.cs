using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTeamPopUp : MonoBehaviour
{
    private RectTransform rectTransform;
    private const float timeToMoveIntoCenter = 1f;
    private List<ChipSO> teamMembers;

    [SerializeField] private Button confirmButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text inputField;

    public void Initialize(List<ChipSO> teamMembers)
    {
        this.teamMembers = teamMembers;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.LeanMove(Vector3.zero, timeToMoveIntoCenter);
    }

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(() => CreateTeam());
        exitButton.onClick.AddListener(() => Exit());
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void CreateTeam()
    {
        if (inputField.text == "" || string.IsNullOrWhiteSpace(inputField.text)) { return; }

        Team team = new Team(teamMembers, inputField.text);
        SelectTeamsUI.Instance.AddToTeamsList(team);
        SelectTeamsUI.Instance.SaveNewTeam();
        Exit();
    }

    private void Exit()
    {
        rectTransform.LeanMove(new Vector3(0, -800, 0), timeToMoveIntoCenter);
        Destroy(gameObject, timeToMoveIntoCenter);
    }
}
