using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TMP_Text GameOverText;

    private void Start()
    {
        GameOverText.gameObject.SetActive(false);

        GameTourManager.Instance.OnGameOver += GameTourManager_OnGameOver;
    }

    private void GameTourManager_OnGameOver(object sender, System.EventArgs e)
    {
        SetGameOverText();
    }

    private void SetGameOverText()
    {
        GameOverText.gameObject.SetActive(true);
        GameOverText.text = $"Team {GameTourManager.Instance.GetWinningTeamID()} Won!";
    }
}
