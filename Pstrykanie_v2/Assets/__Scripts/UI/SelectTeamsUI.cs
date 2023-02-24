using System;
using System.Collections;
using System.Collections.Generic;
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
            OnViewUpdate?.Invoke(this, EventArgs.Empty);
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
            Transform slotTransform = Instantiate(slotPrefab, chipsContainerArchon);
            Transform chipTransform = Instantiate(chipPrefab, slotTransform);

            chipTransform.GetComponent<RectTransform>().transform.position = slotTransform.GetComponent<RectTransform>().transform.position;
            
            ChipObject chipObject = chipTransform.GetComponentInChildren<ChipObject>();
            chipObject.Initialize(chip);
        }
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
