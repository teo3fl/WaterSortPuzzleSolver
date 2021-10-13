using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorBall : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;
    public Canvas canvas;
    public Color Color
    {
        set { image.color = value; }
    }

    public ColorSample source;


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down on ball");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag ball");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta * canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag ball");
        Destroy(gameObject);
    }
}
