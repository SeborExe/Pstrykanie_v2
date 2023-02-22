using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button FastGameButton;

    [Header("GameObjects")]
    [SerializeField] private GameObject fastGameGameObjectUI;

    private void OnEnable()
    {
        FastGameButton.onClick.AddListener(() => SelectOption(fastGameGameObjectUI));
    }

    private void OnDisable()
    {
        FastGameButton.onClick.RemoveAllListeners();
    }

    private void SelectOption(GameObject option)
    {
        option.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
