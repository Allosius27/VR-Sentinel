using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTransitionner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Color32 normalColor = Color.white;
    public Color32 hoverColor = Color.grey;
    public Color32 downColor = Color.white;

    private Image image = null;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Enter");

        if(image != null)
            image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("Exit");

        if (image != null)
            image.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("Down");

        if (image != null)
            image.color = downColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("Up");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Click");

        if (image != null)
            image.color = hoverColor;
    }
}
