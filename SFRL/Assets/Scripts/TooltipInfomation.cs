using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TooltipInfomation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerPassive playerPassive;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip_Static("<size=+6>" + playerPassive.name + "</size>" + "\n" + playerPassive.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }
}
