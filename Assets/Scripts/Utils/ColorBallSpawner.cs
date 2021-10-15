using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ColorBallSpawner : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        var colorBallGO = InstantiateColorBall();
        colorBallGO.transform.position = eventData.position;
        eventData.pointerDrag = colorBallGO;

        Debug.Log("Begin drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down");
    }

    protected abstract GameObject InstantiateColorBall();
}
