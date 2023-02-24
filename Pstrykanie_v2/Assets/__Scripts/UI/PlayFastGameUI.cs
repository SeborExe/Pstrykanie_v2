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
        SelectTeamsUI.Instance.OnViewUpdate += SelectTeamsUI_OnViewUpdate;
    }

    private void SelectTeamsUI_OnViewUpdate(object sender, EventArgs e)
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
        UpdateChipsView(teamOne, 1);
        UpdateChipsView(teamTwo, 2);
    }

    private void UpdateChipsView(Image[] imagesArray, int teamNumber)
    {
        for (int i = 0; i < imagesArray.Length; i++)
        {
            if (GameSettings.Instance.GetTeamChips(teamNumber).Count == 0)
            {
                imagesArray[0].color = Color.black;
                imagesArray[1].color = Color.black;
                imagesArray[2].color = Color.black;
            }
            else if (GameSettings.Instance.GetTeamChips(teamNumber).Count == 1)
            {
                imagesArray[0].sprite = GameSettings.Instance.GetTeamChips(teamNumber)[0].Image;
                imagesArray[0].color = Color.white;
                imagesArray[1].color = Color.black;
                imagesArray[2].color = Color.black;
            }
            else if (GameSettings.Instance.GetTeamChips(teamNumber).Count == 2)
            {
                imagesArray[0].sprite = GameSettings.Instance.GetTeamChips(teamNumber)[0].Image;
                imagesArray[1].sprite = GameSettings.Instance.GetTeamChips(teamNumber)[1].Image;
                imagesArray[0].color = Color.white;
                imagesArray[1].color = Color.white;
                imagesArray[2].color = Color.black;
            }
            else if (GameSettings.Instance.GetTeamChips(teamNumber).Count == 3)
            {
                imagesArray[0].sprite = GameSettings.Instance.GetTeamChips(teamNumber)[0].Image;
                imagesArray[1].sprite = GameSettings.Instance.GetTeamChips(teamNumber)[1].Image;
                imagesArray[2].sprite = GameSettings.Instance.GetTeamChips(teamNumber)[2].Image;
                imagesArray[0].color = Color.white;
                imagesArray[1].color = Color.white;
                imagesArray[2].color = Color.white;
            }
        }
    }
}
