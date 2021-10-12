using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorSample : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Image image;
    public Color Color
    {
        get { return image.color; }
        set { image.color = value; }
    }

    public ColorContainer container;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void Delete()
    {
        container.DeleteElement(gameObject);
    }

    public void ChangeColor()
    {
        container.RequestcolorChange(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var colorBall = Instantiate(container.go_colorBall, container.canvas.transform);
        colorBall.transform.position = eventData.position;
        colorBall.GetComponent<ColorBall>().Color = Color;
        colorBall.GetComponent<ColorBall>().canvas = container.canvas;

        eventData.pointerDrag = colorBall;

        Debug.Log("Begin drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
