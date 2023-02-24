using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipTeamSlot : MonoBehaviour, IDropHandler
{
    private ChipSO chipInTeamSlot;
    private bool isEmpty = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<ChipSelectObject>(out ChipSelectObject chip) && isEmpty)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().transform.position = GetComponent<RectTransform>().transform.position;
            chipInTeamSlot = chip.GetChipData();
            isEmpty = false;
        }
    }

    public ChipSO GetChipInTeamSlot()
    {
        return chipInTeamSlot;
    }
}
