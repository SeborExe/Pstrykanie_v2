using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipSelectObject : MonoBehaviour
{
    [SerializeField] private Image chipIcon;

    public void Initialize(ChipSO chip)
    {
        chipIcon.sprite = chip.Image;
    }
}
