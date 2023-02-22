using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] TMP_Text currentGameStateText;
    [SerializeField] TMP_Text changingGameStateText;

    private void Start()
    {
        GameTourManager.Instance.OnStateChanged += GameTourManager_OnStateChanged;
        GameTourManager.Instance.OnNextStateChanged += GameTourManager_OnNextStateChanged;

        RefreshUI();
        ChangeChangingGameStateTextState(false);
    }

    private void GameTourManager_OnNextStateChanged(object sender, GameTourManager.OnNextStateChangedArgs args)
    {
        if (args.nextGameState == GameState.TeamOneTurn)
        {
            ChangeChangingGameStateTextState(true, "Team 1");
        }

        else if (args.nextGameState == GameState.TeamTwoTurn)
        {
            ChangeChangingGameStateTextState(true, "Team 2");
        }
        else
        {
            ChangeChangingGameStateTextState(false);
        }
    }

    private void GameTourManager_OnStateChanged(object sender, System.EventArgs e)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamOneTurn)
        {
            currentGameStateText.text = "Team 1";
        }
        else if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamTwoTurn || GameTourManager.Instance.GetNextGameState() == GameState.TeamTwoTurn)
        {
            currentGameStateText.text = "Team 2";
        }
        else
        {
            currentGameStateText.text = string.Empty;
        }
    }

    private void ChangeChangingGameStateTextState(bool show, string text = "")
    {
        changingGameStateText.gameObject.SetActive(show);
        changingGameStateText.text = text;
    }
}
