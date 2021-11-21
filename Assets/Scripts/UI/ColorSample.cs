using UnityEngine;
using UnityEngine.UI;

public class ColorSample : ColorBallSpawner
{
    public int ID { get; set; } = 1;
    [SerializeField]
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


    public void Delete()
    {
        container.DeleteElement(gameObject);
    }

    public void ChangeColor()
    {
        container.RequestcolorChange(this);
    }

    protected override GameObject InstantiateColorBall()
    {
        var colorBallGO = Instantiate(container.go_colorBall, container.canvas.transform);
        var colorBall = colorBallGO.GetComponent<ColorBall>();
        colorBall.Initialize(this);

        return colorBallGO;
    }

    private void OnDestroy()
    {
        onDelete?.Invoke();
    }

    public ColorSampleData GetData()
    {
        return new ColorSampleData() 
        { 
            id = ID, 
            r = Color.r,
            g = Color.g,
            b = Color.b,
            a = Color.a,
        };
    }

    public void SetData(ColorSampleData data)
    {
        onColorChanged = null;
        onDelete = null;

        ID = data.id;
        Color = new Color(data.r, data.g, data.b, data.a);
    }
}
