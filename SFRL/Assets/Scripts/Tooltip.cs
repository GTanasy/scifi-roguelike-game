using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    static Tooltip instance;

    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private Camera uiCamera;
    RectTransform rectTransform;
    RectTransform backgroundRectTransform;
    TMP_Text tooltipText;

    void Awake()
    {
        instance = this;
        rectTransform = transform.GetComponent<RectTransform>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("TooltipText").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        if (localPoint.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width / 2)
        {
            localPoint.x = canvasRectTransform.rect.width / 2 - backgroundRectTransform.rect.width;
        }
        if (localPoint.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height / 2)
        {
            localPoint.y = canvasRectTransform.rect.height / 2 - backgroundRectTransform.rect.height;
        }
        transform.localPosition = localPoint;
    }
    void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        tooltipText.ForceMeshUpdate();
        Vector2 backgroundSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = backgroundSize + paddingSize;
    }

    void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipString)
    {
        instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
