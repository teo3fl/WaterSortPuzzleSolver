using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorContainer : ContainerManager
{
    public static ColorContainer Instance { get; private set; }

    [SerializeField]
    private GameObject go_colorSample;
    [SerializeField]
    private FlexibleColorPicker colorPicker;

    public GameObject go_colorBall;
    public Canvas canvas;

    private ColorSample lastPickedSample;

    private int idCounter = 1;


    private void Start()
    {
        Instance = this;
    }

    public Color GetColorBySampleId(int id)
    {
        foreach (GameObject child in t_container)
        {
            var sample = child.GetComponent<ColorSample>();
            if (sample.ID == id)
            {
                return sample.Color;
            }
        }

        throw new System.Exception($"Unable to find color sample with id = {id}.");
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
