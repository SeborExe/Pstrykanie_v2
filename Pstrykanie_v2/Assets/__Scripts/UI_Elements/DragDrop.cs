using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 offset;
    private Transform rootparent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rootparent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        offset = (Vector2)rectTransform.position - eventData.position;

        transform.parent = GetComponentInParent<SelectTeamsUI>().transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position + (Vector2)offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.parent = rootparent;

        Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit);

        if (hit.collider == null)
        {
            ChipObject chipObject = GetComponent<ChipObject>();
            if (chipObject.PreviousPositionIsChipTeamSlot())
            {
                chipObject.SetPreviousPositionDataToNull();
                Destroy(gameObject);
            }
            else
            {
                chipObject.BackToPreviousPosition();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
