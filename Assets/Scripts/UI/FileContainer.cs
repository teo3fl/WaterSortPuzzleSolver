using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileContainer : ContainerManager
{
    [SerializeField]
    private GameObject go_listElement;

    private ScrollListButton lastSelected;
    private ScrollListButton LastSelected
    {
        get { return lastSelected; }
        set
        {
            if (lastSelected)
                lastSelected.Deselect();

            lastSelected = value;
        }
    }

    public string SelectedItem
    {
        get
        {
            if (lastSelected)
                return lastSelected.fileName;
            return string.Empty;
        }
    }

    public delegate void ElementSelected();
    public ElementSelected onElementSelected;


    public void Display(List<string> fileNames)
    {
        foreach (string file in fileNames)
        {
            AddElement(file);
        }

        ResizeContainerHeight();
    }

    private void AddElement(string text)
    {
        var button = Instantiate(go_listElement, t_container).GetComponent<ScrollListButton>();
        button.Initialize(text, this);
    }

    public void DeleteSelectedElement()
    {
        DeleteElement(LastSelected.gameObject);
    }

    public void Select(ScrollListButton button)
    {
        LastSelected = button;

        onElementSelected?.Invoke();
    }

    public void Deselect()
    {
        LastSelected = null;
    }

    public override void ResetContents()
    {
        for (int i = 0; i < t_container.childCount; ++i)
        {
            Destroy(t_container.GetChild(i).gameObject);
        }
    }

    protected override float GetContentHeight()
    {
        if (t_container.childCount == 0)
            return 0;

        var elementHeight = t_container.GetChild(0).GetComponent<RectTransform>().rect.height;

        return t_container.childCount * elementHeight;
    }

    protected override void InstantiateElement()
    {
        throw new System.NotImplementedException();
    }
}
