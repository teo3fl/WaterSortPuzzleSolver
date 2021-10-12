using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeakerContainer : ContainerManager
{
    [SerializeField]
    private GameObject go_beaker;


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
        var beaker = Instantiate(go_beaker, t_container).GetComponent<Beaker>();
        beaker.container = this;
    }
}
