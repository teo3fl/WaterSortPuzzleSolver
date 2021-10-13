using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorSample : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int ID { get; set; } = 1;
    private Image image;
    public Color Color
    {
        get { return image.color; }
        set 
        { 
            image.color = value;
            onColorChanged?.Invoke(value);
        }
    }

    public delegate void ColorChangeHandler(Color newColor);
    public delegate void DeleteHandler();
    public ColorChangeHandler onColorChanged;
    public DeleteHandler onDelete;

    public ColorContainer container;


    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void Delete()
    {
        onDelete?.Invoke();
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
        var colorBallPrefab = Instantiate(container.go_colorBall, container.canvas.transform);
        colorBallPrefab.transform.position = eventData.position;

        var colorBall = colorBallPrefab.GetComponent<ColorBall>();
        colorBall.Color = Color;
        colorBall.source = this;
        colorBall.canvas = container.canvas;

        eventData.pointerDrag = colorBallPrefab;

        Debug.Log("Begin drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
