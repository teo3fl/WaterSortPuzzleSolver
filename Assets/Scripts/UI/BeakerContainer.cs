using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeakerContainer : ContainerManager
{
    public static BeakerContainer Instance { get; private set; }

    [SerializeField]
    private GameObject go_beaker;
    [SerializeField]
    private ColorSample defaultColorSample;
    public ColorSample DefaultColorSample { get { return defaultColorSample; } }

    [SerializeField]
    private Slider sl_beakerCapacity;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            var rectTransform = t_container.GetComponent<RectTransform>();
            f_initialContainerHeight = rectTransform.rect.height;
        }
    }

    protected override float GetContentHeight()
    {
        // numberOfRows = (childCount * cellSize.x + (childCount - 1 ) * gridLayoutGroup.spacing.x) / width
        // height = numberOfRows * cellSize.y + (numberOfRows - 1) * gridLayoutGroup.spacing.y

        var gridLayout = t_container.GetComponent<GridLayoutGroup>();
        var cellSize = gridLayout.cellSize;
        int childCount = t_container.childCount;

        int numberOfRows = Mathf.FloorToInt((childCount * cellSize.x + (childCount - 1) * gridLayout.spacing.x) / t_container.GetComponent<RectTransform>().rect.width) + 1;

        return numberOfRows * cellSize.y + (numberOfRows - 1) * gridLayout.spacing.y;
    }

    protected override void InstantiateElement()
    {
        var beaker = Instantiate(go_beaker, t_container).GetComponent<BeakerUI>();
        beaker.container = this;
        beaker.Initialize();
    }

    private void InstantiateElement(Beaker data)
    {
        var beaker = Instantiate(go_beaker, t_container).GetComponent<BeakerUI>();
        beaker.container = this;
        beaker.Initialize();
        beaker.SetData(data);
    }

    public List<Beaker> GetData()
    {
        var list = new List<Beaker>();
        for (int i = 0; i < t_container.childCount - 1; ++i)
        {
            list.Add(t_container.GetChild(i).GetComponent<BeakerUI>().GetData());
        }

        return list;
    }

    public void LoadData(List<Beaker> beakers, int maxCapacity)
    {
        ResetContents();
        sl_beakerCapacity.value = maxCapacity;

        foreach (var beakerData in beakers)
        {
            InstantiateElement(beakerData);
        }

        OnContentCountChanged();
        t_addButton.SetSiblingIndex(t_container.childCount - 1);

        StartCoroutine(SetScrollBarValue(0f));
    }

    public override void ResetContents()
    {
        for (int i = 0; i < t_container.childCount - 1; ++i)
        {
            Destroy(t_container.GetChild(i).gameObject);
        }
    }
}
