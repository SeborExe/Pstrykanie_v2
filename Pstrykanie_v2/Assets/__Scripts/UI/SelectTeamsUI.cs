using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTeamsUI : MonoBehaviour
{
    [SerializeField] private Transform chipsContainerArchon;
    [SerializeField] private ChipSelectObject chipPrefab;
    [SerializeField] private List<ChipSO> allchips = new List<ChipSO>();
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject fastGameUI;

    private void Start()
    {
        InitializeUI();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        backButton.onClick.AddListener(() => SelectOption(fastGameUI));
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
            ChipSelectObject chipObject = Instantiate(chipPrefab, chipsContainerArchon);
            chipObject.Initialize(chip);
        }
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
