using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFastGameUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button selectMapButton;
    [SerializeField] private Button backToMainMenuButton;

    [Header("GameObject")]
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject selectMapGameObjectUI;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        backToMainMenuButton.onClick.AddListener(() => SelectOption(mainMenuGameObject));
        playButton.onClick.AddListener(() => PlayGame());
        selectMapButton.onClick.AddListener(() => SelectOption(selectMapGameObjectUI));
    }

    private void OnDisable()
    {
        backToMainMenuButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
        selectMapButton.onClick.RemoveAllListeners();
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(GameSettings.Instance.GetCurrentSelectedMap().mapSceneName);
    }
}
