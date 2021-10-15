using UnityEngine;
using UnityEngine.UI;

public class ViewOnlyBeakerContent : MonoBehaviour
{
    public void Initialize(int colorId)
    {
        //set color
        var image = GetComponent<Image>();
        image.color = ColorContainer.Instance.GetColorBySampleId(colorId);

        // set size
        var rectTransform = GetComponent<RectTransform>();
        var width = rectTransform.sizeDelta.x;
        rectTransform.sizeDelta = new Vector2(width, BeakerUI.ContentHeight);
    }
}
