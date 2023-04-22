using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    private GameTourManager gameTourManager;
    private GameManager gameManager;

    [Header("Current GameState")]
    [SerializeField] TMP_Text currentGameStateText;
    [SerializeField] TMP_Text changingGameStateText;
    [Header("Chip to Place")]
    [SerializeField] GameObject teamOnePlace;
    [SerializeField] GameObject teamTwoPlace;
    [SerializeField] Image teamOneChipToPlace;
    [SerializeField] Image teamTwoChipToPlace;

    private void Start()
    {
        gameTourManager = GameTourManager.Instance;
        gameManager = GameManager.Instance;

        gameTourManager.OnStateChanged += GameTourManager_OnStateChanged;
        gameTourManager.OnNextStateChanged += GameTourManager_OnNextStateChanged;
        gameTourManager.OnPlacingChipEnded += GameTourManager_OnPlacingChipEnded;

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

    private void GameTourManager_OnPlacingChipEnded(object sender, EventArgs e)
    {
        teamOnePlace.gameObject.SetActive(false);
        teamTwoPlace.gameObject.SetActive(false);
    }

    private void RefreshUI()
    {
        if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamOneTurn || GameTourManager.Instance.GetNextGameState() == GameState.TeamOneTurn)
        {
            currentGameStateText.text = "Team 1";
        }
        else if (GameTourManager.Instance.GetCurrentGameState() == GameState.TeamTwoTurn || GameTourManager.Instance.GetNextGameState() == GameState.TeamTwoTurn)
        {
            currentGameStateText.text = "Team 2";
        }
        if (GameTourManager.Instance.GetCurrentGameState() == GameState.PlacingChipsByTeamOne || GameTourManager.Instance.GetNextGameState() == GameState.PlacingChipsByTeamOne)
        {
            currentGameStateText.text = "Team 1 Placing";
            ShowNextChip(1);
        }
        else if (GameTourManager.Instance.GetCurrentGameState() == GameState.PlacingChipsByTeamTwo || GameTourManager.Instance.GetNextGameState() == GameState.PlacingChipsByTeamTwo)
        {
            currentGameStateText.text = "Team 2 Placing";
            ShowNextChip(2);
        }
        else
        {
            currentGameStateText.text = string.Empty;
        }
    }

    private void ShowNextChip(int team)
    {
        if (team == 1)
        {
            teamOnePlace.gameObject.SetActive(true);
            teamTwoPlace.gameObject.SetActive(false);
            teamOneChipToPlace.sprite = gameManager.GetNextChipToPlace(1).Image;
        }
        else
        {
            teamTwoPlace.gameObject.SetActive(true);
            teamOnePlace.gameObject.SetActive(false);
            teamTwoChipToPlace.sprite = gameManager.GetNextChipToPlace(2).Image;
        }
    }

    private void ChangeChangingGameStateTextState(bool show, string text = "")
    {
        changingGameStateText.gameObject.SetActive(show);
        changingGameStateText.text = text;
    }
}
