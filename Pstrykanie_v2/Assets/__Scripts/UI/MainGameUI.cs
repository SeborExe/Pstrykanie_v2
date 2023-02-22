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
        GameTourManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        RefreshUI();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamOneTurn || GameTourManager.Instance.GetNextGameState() == GameState.TeamOneTurn)
        {
            currentGameStateText.text = "Team 1 Turn";
        }
        else if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamTwoTurn || GameTourManager.Instance.GetNextGameState() == GameState.TeamTwoTurn)
        {
            currentGameStateText.text = "Team 2 Turn";
        }
        else
        {
            currentGameStateText.text = string.Empty;
        }
    }
}
