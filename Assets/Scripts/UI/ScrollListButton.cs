using UnityEngine;
using UnityEngine.UI;

public class ScrollListButton : MonoBehaviour
{
    [SerializeField]
    private Color normal;
    [SerializeField]
    private Color highlighted;
    [SerializeField]
    private Color selected;

    private Image background;

    public bool isSelected = false;
    public string fileName;

    private FileContainer container;


    public void Initialize(string text)
    {
        fileName = text;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        background = GetComponent<Image>();

        SetColor(normal);
    }

    public void Deselect()
    {
        SetColor(normal);
        isSelected = false;
    }

    private void OnMouseUp()
    {
        container.Select(this);
        SetColor(selected);
        isSelected = true;
    }

    private void OnMouseEnter()
    {
        if(!isSelected)
        {
            SetColor(highlighted);
        }
        else
        {
            container.Select(null);
        }
    }

    private void OnMouseExit()
    {
        if (!isSelected)
        {
            SetColor(normal);
        }
    }

    private void SetColor(Color color)
    {
        background.color = color;
    }
}
