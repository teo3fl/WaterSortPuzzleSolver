using UnityEngine;
using UnityEngine.UI;

public class ColorSample : ColorBallSpawner
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

    protected override GameObject InstantiateColorBall()
    {
        var colorBallGO = Instantiate(container.go_colorBall, container.canvas.transform);
        var colorBall = colorBallGO.GetComponent<ColorBall>();
        colorBall.Initialize(this);

        return colorBallGO;
    }
}
