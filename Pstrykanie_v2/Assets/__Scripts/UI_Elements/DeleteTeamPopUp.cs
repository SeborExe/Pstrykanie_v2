using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteTeamPopUp : MonoBehaviour
{
    private RectTransform rectTransform;
    private const float timeToMoveIntoCenter = 0.5f;
    private Team team;

    [SerializeField] private Button confirmButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text teamName;

    public void Initialize(Team team)
    {
        this.team = team;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.LeanMove(Vector3.zero, timeToMoveIntoCenter);
        teamName.text = team.teamName;
    }

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(() => DeleteTeam());
        exitButton.onClick.AddListener(() => Exit());
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void DeleteTeam()
    {
        SelectTeamsUI.Instance.RemoveTeam(team);
        Exit();
    }

    private void Exit()
    {
        rectTransform.LeanMove(new Vector3(0, -800, 0), timeToMoveIntoCenter);
        Destroy(gameObject, timeToMoveIntoCenter);
    }
}
