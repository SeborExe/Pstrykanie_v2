using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectOption : MonoBehaviour
{
    public event EventHandler OnMapChanged;

    [SerializeField] TMP_Text mapName;
    private Button mapButton;
    private Image backgroundImage;
    private Image buttonMapImage;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        mapButton = GetComponentInChildren<Button>();
        buttonMapImage = mapButton.GetComponent<Image>();
    }

    private void OnEnable()
    {
        mapButton.onClick.AddListener(() =>
        {
            OnMapChanged?.Invoke(this, EventArgs.Empty);
        });
    }

    private void OnDisable()
    {
        mapButton.onClick.RemoveAllListeners();
    }

    public void Initialize(MapSO map)
    {
        mapName.text = map.name;
        buttonMapImage.sprite = map.mapIcon;

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
