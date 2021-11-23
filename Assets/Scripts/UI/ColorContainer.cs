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


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateBaseContainerHeight();
        }
    }

    public Color GetColorBySampleId(int id)
    {
        return GetSampleById(id).Color;
    }

    public ColorSample GetSampleById(int id)
    {
        for (int i = 0; i < t_container.childCount; ++i)
        {
            var sample = t_container.GetChild(i).GetComponent<ColorSample>();
            if (sample.ID == id)
            {
                return sample;
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

    protected void InstantiateElement(ColorSampleData data)
    {
        var sample = Instantiate(go_colorSample, t_container).GetComponent<ColorSample>();
        sample.container = this;
        sample.SetData(data);
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

    public override void ResetContents()
    {
        for (int i = 1; i < t_container.childCount - 1; ++i)
        {
            Destroy(t_container.GetChild(i).gameObject);
        }
    }

    public List<ColorSampleData> GetData()
    {
        var list = new List<ColorSampleData>();
        for (int i = 0; i < t_container.childCount - 1; ++i)
        {
            list.Add(t_container.GetChild(i).GetComponent<ColorSample>().GetData());
        }

        return list;
    }

    public void LoadData(List<ColorSampleData> colors)
    {
        ResetContents();

        var defaultSample = t_container.GetChild(0).GetComponent<ColorSample>();
        defaultSample.SetData(colors[0]);

        for (int i = 1; i < colors.Count; i++)
        {
            InstantiateElement(colors[i]);
        }

        StartCoroutine(UpdateContainerHeight(0.1f));

        idCounter = colors[colors.Count - 1].id;
    }
}
