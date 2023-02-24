using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTeamsUI : SingletonMonobehaviour<SelectTeamsUI>
{
    public event EventHandler OnSelectTeamConfirm;

    [SerializeField] private Transform chipsContainerArchon;
    [SerializeField] private Transform chipPrefab;
    [SerializeField] private List<ChipSO> allchips = new List<ChipSO>();
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject fastGameUI;

    protected override void Awake()
    {
        base.Awake();
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
            SelectOption(fastGameUI);
        });
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
    }

    private void InitializeUI()
    {
        foreach (Transform child in chipsContainerArchon)
        {
            Destroy(child.gameObject);
        }

        foreach (ChipSO chip in allchips)
        {
            Transform chipTransform = Instantiate(chipPrefab, chipsContainerArchon);
            ChipSelectObject chipObject = chipTransform.GetComponentInChildren<ChipSelectObject>();
            chipObject.Initialize(chip);
        }
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
