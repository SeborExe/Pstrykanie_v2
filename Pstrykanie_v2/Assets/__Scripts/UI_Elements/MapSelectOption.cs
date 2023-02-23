using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectOption : MonoBehaviour
{
    [SerializeField] TMP_Text mapName;
    private Button mapButton;
    private Image backgroundImage;
    private Image buttonMapImage;
    private MapSO map;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        mapButton = GetComponentInChildren<Button>();
        buttonMapImage = mapButton.GetComponent<Image>();

        SelectMapUI.OnMapChanged += SelectMapUI_OnMapChanged;
    }

    private void SelectMapUI_OnMapChanged(object sender, EventArgs e)
    {
        UpdateUIChanges(map);
    }

    private void OnEnable()
    {
        mapButton.onClick.AddListener(() =>
        {
            GameSettings.Instance.SetSelectedMap(map);
            SelectMapUI.OnMapChangedInvoke(this);
        });
    }

    private void OnDisable()
    {
        mapButton.onClick.RemoveAllListeners();
    }

    public void Initialize(MapSO map)
    {
        this.map = map;
        mapName.text = map.name;
        buttonMapImage.sprite = map.mapIcon;

        UpdateUIChanges(map);
    }

    private void UpdateUIChanges(MapSO map)
    {
        if (GameSettings.Instance.GetCurrentSelectedMap().name == map.name)
        {
            backgroundImage.color = Color.green;
        }
        else
        {
            backgroundImage.color = Color.white;
        }
    }
}
