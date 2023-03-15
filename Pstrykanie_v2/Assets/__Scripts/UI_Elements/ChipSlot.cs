using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<ChipObject>(out ChipObject chip))
        {
            if (chip.PreviousPositionIsChipTeamSlot())
            {
                chip.SetPreviousPositionDataToNull();
                Destroy(chip.gameObject);
            }
            else
            {
                chip.BackToPreviousPosition();
            }
        }
    }
}
