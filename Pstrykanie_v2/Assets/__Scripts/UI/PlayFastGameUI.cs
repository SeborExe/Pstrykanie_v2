using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFastGameUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button selectMapButton;
    [SerializeField] private Button selectTeamsButton;
    [SerializeField] private Button backToMainMenuButton;

    [Header("GameObject")]
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject selectMapGameObjectUI;
    [SerializeField] private GameObject selectTeamsUI;

    [Header("Selected Map")]
    [SerializeField] private Image mapPreviewImage;
    [SerializeField] private TMP_Text mapText;

    [Header("Selected Teams")]
    [SerializeField] private Image[] teamOne;
    [SerializeField] private Image[] teamTwo;

    private void Start()
    {
        UpdateMapPreview();
        gameObject.SetActive(false);

        ChanePlayButtonState();
        UpdateTeamsView();

        SelectMapUI.OnMapChanged += SelectMapUI_OnMapChanged;
        SelectTeamsUI.Instance.OnSelectTeamConfirm += SelectTeamsUI_OnSelectTeamConfirm;
    }

    private void SelectTeamsUI_OnSelectTeamConfirm(object sender, EventArgs e)
    {
        ChanePlayButtonState();
        UpdateTeamsView();
    }

    private void OnEnable()
    {
        backToMainMenuButton.onClick.AddListener(() => SelectOption(mainMenuGameObject));
        playButton.onClick.AddListener(() => PlayGame());
        selectMapButton.onClick.AddListener(() => SelectOption(selectMapGameObjectUI));
        selectTeamsButton.onClick.AddListener(() => SelectOption(selectTeamsUI));
    }

    private void OnDisable()
    {
        backToMainMenuButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
        selectMapButton.onClick.RemoveAllListeners();
        selectTeamsButton.onClick.RemoveAllListeners();
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

    private void UpdateMapPreview()
    {
        mapPreviewImage.sprite = GameSettings.Instance.GetCurrentSelectedMap().mapIcon;
        mapText.text = GameSettings.Instance.GetCurrentSelectedMap().mapName;
    }

    private void SelectMapUI_OnMapChanged(object sender, EventArgs e)
    {
        UpdateMapPreview();
    }

    private void ChanePlayButtonState()
    {
        playButton.enabled = GameSettings.Instance.TeamsHasEnoughMembers();

        if (!playButton.enabled)
        {
            playButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            playButton.GetComponent<Image>().color = Color.green;
        }
    }

    private void UpdateTeamsView()
    {
        for (int i = 0; i < teamOne.Length; i++)
        {
            if (GameSettings.Instance.GetTeamChips(1).Count != 0)
            {
                teamOne[i].sprite = GameSettings.Instance.GetTeamChips(1)[i].Image;
            }
            else
            {
                teamOne[i].sprite = null;
            }
        }
        for (int i = 0; i < teamTwo.Length; i++)
        {
            if (GameSettings.Instance.GetTeamChips(2).Count != 0)
            {
                teamTwo[i].sprite = GameSettings.Instance.GetTeamChips(2)[i].Image;
            }
            else
            {
                teamTwo[i].sprite = null;
            }
        }
    }
}
