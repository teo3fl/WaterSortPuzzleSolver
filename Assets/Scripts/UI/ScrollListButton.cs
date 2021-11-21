using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollListButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
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


    public void Initialize(string text, FileContainer fileContainer)
    {
        fileName = text;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        background = GetComponent<Image>();
        container = fileContainer;

        SetColor(normal);
    }

    public void Deselect()
    {
        SetColor(normal);
        isSelected = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isSelected)
        {
            container.Deselect();
        }
        else
        {
            container.Select(this);
            SetColor(selected);
            isSelected = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            SetColor(highlighted);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
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

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
