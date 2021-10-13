using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeakerContent : MonoBehaviour, IDropHandler
{
    private Image image;
    private RectTransform rectTransform;

    public Beaker beaker;
    private ColorSample colorSource;
    private ColorSample ColorSource
    {
        get { return colorSource; }
        set
        {
            if (colorSource)
            {
                colorSource.onColorChanged -= OnColorSourceChangeColor;
                colorSource.onDelete -= OnColorSourceDeletion;
            }

            colorSource = value;

            colorSource.onColorChanged += OnColorSourceChangeColor;
            colorSource.onDelete += OnColorSourceDeletion;

            image.color = colorSource.Color;
        }
    }

    private float f_width;

    public void Initialize(float height)
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        f_width = rectTransform.sizeDelta.x;

        Resize(height);
    }

    public void Resize(float height)
    {
        rectTransform.sizeDelta = new Vector2(f_width, height);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag)
        {
            ColorSource = eventData.pointerDrag.GetComponent<ColorBall>().source;
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    private void OnColorSourceChangeColor(Color newColor)
    {
        image.color = newColor;
    }

    private void OnColorSourceDeletion()
    {
        Delete();
    }
}
