using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class SelectTeamsUI : SingletonMonobehaviour<SelectTeamsUI>
{
    public event EventHandler OnSelectTeamConfirm;
    public event EventHandler OnViewUpdate;

    [SerializeField] private Transform chipsContainerArchon;
    [SerializeField] private Transform chipPrefab;
    [SerializeField] private Transform slotPrefab;
    [SerializeField] private List<ChipSO> allchips = new List<ChipSO>();
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject fastGameUI;

    [Header("Save Team")]
    [SerializeField] private Button saveTeam_1_Button;
    [SerializeField] private Button saveTeam_2_Button;
    [SerializeField] private Transform savedTeamsContainerArchon;
    [SerializeField] private TeamObject teamObjectPrefab;
    [SerializeField] private TeamSelect teamSelectOne;
    [SerializeField] private TeamSelect teamSelectTwo;

    private List<Team> teams = new List<Team>();
    private const string fileName = "/saveFile.json";

    protected override void Awake()
    {
        base.Awake();

        teams = FileHandler.ReadFromJSON<Team>(fileName);
    }

    private void Start()
    {
        InitializeUI();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        backButton.onClick.AddListener(() =>
        {
            OnSelectTeamConfirm?.Invoke(this, EventArgs.Empty);
            OnViewUpdate?.Invoke(this, EventArgs.Empty);
            SelectOption(fastGameUI);
            //InitializeSavedTeams();
        });

        saveTeam_1_Button.onClick.AddListener(() => 
        {
            OnSelectTeamConfirm?.Invoke(this, EventArgs.Empty);
            TrySaveTeam(1);
        });

        saveTeam_2_Button.onClick.AddListener(() =>
        {
            OnSelectTeamConfirm?.Invoke(this, EventArgs.Empty);
            TrySaveTeam(2);
        });
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        saveTeam_1_Button.onClick.RemoveAllListeners();
        saveTeam_2_Button.onClick.RemoveAllListeners();
    }

    private void InitializeUI()
    {
        foreach (Transform child in chipsContainerArchon)
        {
            Destroy(child.gameObject);
        }

        foreach (ChipSO chip in allchips)
        {
            Transform slotTransform = Instantiate(slotPrefab, chipsContainerArchon);
            Transform chipTransform = Instantiate(chipPrefab, slotTransform);

            chipTransform.GetComponent<RectTransform>().transform.position = slotTransform.GetComponent<RectTransform>().transform.position;
            
            ChipObject chipObject = chipTransform.GetComponentInChildren<ChipObject>();
            chipObject.Initialize(chip);
        }

        InitializeSavedTeams();
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void TrySaveTeam(int teamID)
    {
        if (!GameSettings.Instance.TeamHasEnoughMembers(teamID)) return;

        List<ChipSO> teamMembers = new List<ChipSO>();

        if (teamID == 1)
        {
            teamMembers.Clear();

            foreach (ChipTeamSlot chip in teamSelectOne.GetTeam())
            {
                if (chip.GetChipInTeamSlot() == null) continue;

                teamMembers.Add(chip.GetChipInTeamSlot());
            }
        }

        if (teamID == 2)
        {
            teamMembers.Clear();

            foreach (ChipTeamSlot chip in teamSelectTwo.GetTeam())
            {
                if (chip.GetChipInTeamSlot() == null) continue;

                teamMembers.Add(chip.GetChipInTeamSlot());
            }
        }

        Team team = new Team(teamMembers);
        teams.Add(team);

        FileHandler.SaveToJSON<Team>(teams, "saveFile.json");

        InitializeSavedTeams();
    }

    private void InitializeSavedTeams()
    {
        foreach(Transform child in savedTeamsContainerArchon)
        {
            Destroy(child.gameObject);
        }

        foreach (Team team in teams)
        {
            TeamObject teamObject = Instantiate(teamObjectPrefab, savedTeamsContainerArchon);
            teamObject.SetTeam(team);
        }
           
    }

    public void LoadTeamFromSaved(int teamID, List<ChipSO> team)
    {
        if (teamID == 1)
        {
            teamSelectOne.SetTeamFromSaved(team);
        }

        if (teamID == 2)
        {
            teamSelectTwo.SetTeamFromSaved(team);
        }

        else return;
    }
}
