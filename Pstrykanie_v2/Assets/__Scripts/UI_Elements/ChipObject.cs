using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChipObject : MonoBehaviour
{
    [SerializeField] private Image chipIcon;
    [SerializeField] private Image metalFrame;
    [SerializeField] private TMP_Text assignText;
    private ChipSO chip;

    public void Initialize(ChipSO chip)
    {
        this.chip = chip;
        chipIcon.sprite = chip.Image;

        if (!chip.isMetal)
        {
            metalFrame.gameObject.SetActive(false);
            assignText.gameObject.SetActive(false);
        }
    }

    public ChipSO GetChipData()
    {
        return chip;
    }

    public void BackToPreviousPosition()
    {
        gameObject.GetComponent<RectTransform>().transform.position = transform.parent.GetComponent<RectTransform>().transform.position;
    }

    public bool PreviousPositionIsChipTeamSlot()
    {
        return transform.parent.GetComponent<ChipTeamSlot>();
    }

    public void SetPreviousPositionDataToNull()
    {
        transform.parent.GetComponent<ChipTeamSlot>().ResetChipData();
    }

    public void SetChipData(ChipSO chip)
    {
        this.chip = chip;
        Initialize(chip);
    }
}
