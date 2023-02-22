using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFastGameUI : MonoBehaviour
{
    private const string MAP_1 = "Map_1";

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button backToMainMenuButton;

    [Header("GameObject")]
    [SerializeField] private GameObject mainMenuGameObject;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        backToMainMenuButton.onClick.AddListener(() => SelectOption(mainMenuGameObject));
        playButton.onClick.AddListener(() => PlayGame(MAP_1));
    }

    private void OnDisable()
    {
        backToMainMenuButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void PlayGame(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }
}
