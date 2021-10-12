using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeakerContent : MonoBehaviour, IDropHandler
{
    private Image image;
    private RectTransform rectTransform;

    public Beaker beaker;

    private float f_width;

    private void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        f_width = rectTransform.sizeDelta.x;
    }

    public void Resize(float height)
    {
        rectTransform.sizeDelta = new Vector2(f_width, height);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag)
        {
            image.color = eventData.pointerDrag.GetComponent<ColorBall>().Color;
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
