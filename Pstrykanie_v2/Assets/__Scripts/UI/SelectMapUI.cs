using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMapUI : MonoBehaviour
{
    [SerializeField] private Transform mapsAnchor;
    [SerializeField] private MapSelectOption mapSelectPrefab;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject fastGameUI;

    private List<MapSO> allMaps = new List<MapSO>();

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
        allMaps = GameSettings.Instance.GetAllMaps();

        foreach (Transform child in mapsAnchor)
        {
            Destroy(child.gameObject);
        }

        foreach (MapSO map in allMaps)
        {
            MapSelectOption mapOption = Instantiate(mapSelectPrefab, mapsAnchor.transform);
            mapOption.Initialize(map);
        }
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
