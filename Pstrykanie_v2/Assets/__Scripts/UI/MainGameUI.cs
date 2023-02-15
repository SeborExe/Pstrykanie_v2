using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] TMP_Text currentGameStateText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        RefreshUI();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        currentGameStateText.text = GameManager.Instance.GetCurrentGameState().ToString();
    }
}
