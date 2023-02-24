using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipTeamSlot : MonoBehaviour, IDropHandler
{
    private ChipSO chipInTeamSlot;

    private void Start()
    {
        SelectTeamsUI.Instance.OnSelectTeamConfirm += SelectTeamsUI_OnSelectTeamConfirm;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<ChipObject>(out ChipObject chip))
        {
            if (gameObject.transform.childCount != 0)
            {
                chip.BackToPreviousPosition();
            }
            else
            {
                if (!chip.PreviousPositionIsChipTeamSlot())
                {
                    chip.BackToPreviousPosition();
                    chipInTeamSlot = chip.GetChipData();
                    ChipObject chipObject = Instantiate(chip, transform);
                    chipObject.GetComponent<RectTransform>().transform.position = GetComponent<RectTransform>().transform.position;
                    chipObject.gameObject.transform.SetParent(this.gameObject.transform);

                    CanvasGroup canvasGroup = chipObject.gameObject.GetComponent<CanvasGroup>();
                    canvasGroup.alpha = 1f;
                    canvasGroup.blocksRaycasts = true;
                }
                else
                {
                    eventData.pointerDrag.GetComponent<RectTransform>().transform.position = GetComponent<RectTransform>().transform.position;
                    chip.SetPreviousPositionDataToNull();
                    chipInTeamSlot = chip.GetChipData();
                    chip.gameObject.transform.SetParent(this.gameObject.transform);
                }
            }
        }
    }

    private void SelectTeamsUI_OnSelectTeamConfirm(object sender, System.EventArgs e)
    {
        if (gameObject.transform.childCount == 0)
        {
            chipInTeamSlot = null;
        }
    }

    public void ResetChipData()
    {
        chipInTeamSlot = null;
    }

    public ChipSO GetChipInTeamSlot()
    {
        return chipInTeamSlot;
    }
}
