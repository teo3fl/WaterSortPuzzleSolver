using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorContainer : ContainerManager
{
    [SerializeField]
    private GameObject go_colorSample;
    [SerializeField]
    private FlexibleColorPicker colorPicker;

    public GameObject go_colorBall;
    public Canvas canvas;

    private ColorSample lastPickedSample;

    private int idCounter = 1;


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

    protected override void InstantiateElement()
    {
        var sample = Instantiate(go_colorSample, t_container).GetComponent<ColorSample>();
        sample.container = this;
        sample.ID = ++idCounter;
    }

    protected override float GetContentHeight()
    {
        float contentHeight = 0f;
        for (int i = 0; i < t_container.childCount; ++i)
        {
            contentHeight += t_container.GetChild(i).GetComponent<RectTransform>().rect.height;
        }

        contentHeight += (t_container.childCount - 1) * t_container.GetComponent<VerticalLayoutGroup>().spacing;

        return contentHeight;
    }
}
