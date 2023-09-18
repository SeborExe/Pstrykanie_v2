using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] TMP_Text GameOverText;
    [SerializeField] private Button rematchButton;
    [SerializeField] private Button backToMenuButton;

    private string currentLevel => SceneManager.GetActiveScene().name;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    private void Start()
    {
        GameTourManager.Instance.OnGameOver += GameTourManager_OnGameOver;
    }

    private void OnEnable()
    {
        rematchButton.onClick.AddListener(() => SceneManager.LoadScene(currentLevel));
        backToMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    private void OnDisable()
    {
        rematchButton.onClick.RemoveAllListeners();
        backToMenuButton.onClick.RemoveAllListeners();
    }

    private void GameTourManager_OnGameOver(object sender, System.EventArgs e)
    {
        SetGameOverText();
    }

    private void SetGameOverText()
    {
        canvasGroup.alpha = 1.0f;
        GameOverText.text = $"Team {GameTourManager.Instance.GetWinningTeamID()} Won!";
    }
}
