using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeakerContainer : ContainerManager
{
    [SerializeField]
    private GameObject go_beaker;
    [SerializeField]
    private ColorSample defaultColorSample;
    public ColorSample DefaultColorSample { get { return defaultColorSample; } }


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

    public List<Beaker> GetBeakers()
    {
        var list = new List<Beaker>();
        for(int i = 0; i< t_container.childCount - 1; ++i)
        {
            list.Add(t_container.GetChild(i).GetComponent<BeakerUI>().GetData());
        }

        return list;
    }
}
