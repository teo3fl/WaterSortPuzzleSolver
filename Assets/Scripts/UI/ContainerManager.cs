using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContainerManager : MonoBehaviour
{
    [SerializeField]
    protected Transform t_container;
    [SerializeField]
    protected Transform t_addButton;
    [SerializeField]
    private Scrollbar scrollBar;


    public void AddElement()
    {
        InstantiateElement();
        SnapElements();
    }

    protected abstract void InstantiateElement();

    public void DeleteElement(GameObject element)
    {
        Destroy(element);
    }

    protected void SnapElements()
    {
        t_addButton.SetSiblingIndex(t_container.childCount - 1);
        StartCoroutine(SetScrollBarValue(0f));
    }

   
    public void OnWindowResize()
    {
        StartCoroutine(SetScrollBarValue(0f));
    }

    protected IEnumerator SetScrollBarValue(float value)
    {
        yield return new WaitForSeconds(0.05f);
        scrollBar.value = value;
    }

    public abstract void ResetContents();
}
