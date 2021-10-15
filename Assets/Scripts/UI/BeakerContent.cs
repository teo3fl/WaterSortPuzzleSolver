using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeakerContent : ColorBallSpawner, IDropHandler
{
    private Image image;
    private RectTransform rectTransform;

    private ColorSample colorSource;
    public ColorSample ColorSource
    {
        get { return colorSource; }
        private set
        {
            if (colorSource)
            {
                colorSource.onColorChanged -= OnColorSourceChangeColor;
                colorSource.onDelete -= OnColorSourceDeletion;
            }

            colorSource = value;

            if (value == null) return;

            colorSource.onColorChanged += OnColorSourceChangeColor;
            colorSource.onDelete += OnColorSourceDeletion;

            image.color = colorSource.Color;
        }
    }

    [SerializeField]
    private GameObject go_colorBall;

    private float f_width;


    public void Initialize(float height, ColorSample colorSample)
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        f_width = rectTransform.sizeDelta.x;
        Resize(height);

        ColorSource = colorSample;
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

    private void OnDestroy()
    {
        ColorSource = null;
        Debug.Log("Destroyed beaker content");
    }

    private void OnColorSourceChangeColor(Color newColor)
    {
        image.color = newColor;
    }

    private void OnColorSourceDeletion()
    {
        Delete();
    }

    protected override GameObject InstantiateColorBall()
    {
        var colorBallGO = Instantiate(colorSource.container.go_colorBall, colorSource.container.canvas.transform);
        var colorBall = colorBallGO.GetComponent<ColorBall>();
        colorBall.Initialize(ColorSource);

        return colorBallGO;
    }
}
