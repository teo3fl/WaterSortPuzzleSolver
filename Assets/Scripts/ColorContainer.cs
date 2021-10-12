using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorContainer : MonoBehaviour
{
    [SerializeField]
    private Transform t_container;
    [SerializeField]
    private GameObject go_colorSample;
    [SerializeField]
    private FlexibleColorPicker colorPicker;
    [SerializeField]
    private Transform t_addButton;
    [SerializeField]
    private Scrollbar scrollBar;

    public GameObject go_colorBall;
    public Canvas canvas;

    private ColorSample lastPickedSample;

    private float f_initialContainerHeight;

    private void Start()
    {
        var rectTransform = t_container.GetComponent<RectTransform>();
        f_initialContainerHeight = rectTransform.rect.height;
    }

    public void AddColor()
    {
        StartCoroutine(AddColorSample());
    }

    private IEnumerator AddColorSample()
    {
        var sample = Instantiate(go_colorSample, t_container).GetComponent<ColorSample>();

        sample.container = this;

        OnContentCountChanged();
        t_addButton.SetSiblingIndex(t_container.childCount - 1);

        yield return SetScrollBarValue(0f);
    }

    public void RequestcolorChange(ColorSample sample)
    {
        lastPickedSample = sample;
        colorPicker.color = sample.Color;
        colorPicker.gameObject.SetActive(true);
    }

    public void PickColor()
    {
        lastPickedSample.Color = colorPicker.color;
        colorPicker.gameObject.SetActive(false);
    }

    public void CancelColorPick()
    {
        colorPicker.gameObject.SetActive(false);
    }

    public void DeleteColor(GameObject sample)
    {
        StartCoroutine(DeleteColorSample(sample));
    }

    private IEnumerator DeleteColorSample(GameObject sample)
    {
        Destroy(sample);
        yield return new WaitForSeconds(0.01f);
        OnContentCountChanged();
    }

    public void OnContentCountChanged()
    {
        // calculate the height of all elements in the container:
        // height = childCount * colorSamplePrefab + (childCount - 1) * verticalLayoutGroup.Spacing
        // if the height of the content is greater than the current height of the container, change the current height 
        // else if the height of the content is lesser, change the height but make it no less than the initial height

        float contentHeight = 0f;
        for(int i =0; i < t_container.childCount; ++i)
        {
            contentHeight += t_container.GetChild(i).GetComponent<RectTransform>().rect.height;
        }

        contentHeight += (t_container.childCount - 1) * t_container.GetComponent<VerticalLayoutGroup>().spacing;

        RectTransform containerRect = t_container.GetComponent<RectTransform>();
        float containerHeight = containerRect.rect.height;

        if (containerHeight < contentHeight)
        {
            containerRect.offsetMin = new Vector2(containerRect.offsetMax.x, containerHeight - contentHeight);
        }
        else
        {
            if(contentHeight >= f_initialContainerHeight)
            {
                float difference = f_initialContainerHeight - contentHeight;
                float scrollBarValue = scrollBar.value;
                containerRect.offsetMax = new Vector2(containerRect.offsetMax.x, 0);
                containerRect.offsetMin = new Vector2(containerRect.offsetMax.x, difference);

                StartCoroutine( SetScrollBarValue(scrollBarValue));
            }
            else
            {
                containerRect.offsetMax = new Vector2(containerRect.offsetMax.x, 0f);
                containerRect.offsetMin = new Vector2(containerRect.offsetMax.x, 0f);
            }
        }
    }

    private IEnumerator SetScrollBarValue(float value)
    {
        yield return new WaitForSeconds(0.001f);
        scrollBar.value = value;
    }
}
