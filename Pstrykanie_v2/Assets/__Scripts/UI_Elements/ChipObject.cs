using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipObject : MonoBehaviour
{
    private Image chipIcon;
    private ChipSO chip;

    private void Awake()
    {
        chipIcon = GetComponentInChildren<Image>();
    }

    public void Initialize(ChipSO chip)
    {
        this.chip = chip;
        chipIcon.sprite = chip.Image;
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
    }
}
