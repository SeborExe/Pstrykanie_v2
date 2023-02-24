using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipSelectObject : MonoBehaviour
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
}
