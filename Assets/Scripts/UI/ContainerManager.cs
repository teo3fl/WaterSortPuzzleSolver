using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContainerManager : MonoBehaviour
{
    [SerializeField]
    protected Transform t_container;
    [SerializeField]
    private Transform t_addButton;
    [SerializeField]
    private Scrollbar scrollBar;

    protected float f_initialContainerHeight;

    private void Start()
    {
        var rectTransform = t_container.GetComponent<RectTransform>();
        f_initialContainerHeight = rectTransform.rect.height;
    }

    public void AddElement()
    {
        InstantiateElement();
        OnContentCountChanged();
        t_addButton.SetSiblingIndex(t_container.childCount - 1);

        StartCoroutine(SetScrollBarValue(0f));
    }

    protected abstract void InstantiateElement();

    public void DeleteElement(GameObject element)
    {
        StartCoroutine(DeleteElementFromList(element));
    }

    private IEnumerator DeleteElementFromList(GameObject element)
    {
        Destroy(element);
        yield return new WaitForSeconds(0.01f);
        OnContentCountChanged();
    }

    public void OnContentCountChanged()
    {
        // calculate the height of all elements in the container:
        // height = childCount * colorSamplePrefab + (childCount - 1) * verticalLayoutGroup.Spacing
        // if the height of the content is greater than the current height of the container, change the current height 
        // else if the height of the content is lesser, change the height but make it no less than the initial height

        float contentHeight = GetContentHeight();

        RectTransform containerRect = t_container.GetComponent<RectTransform>();
        float containerHeight = t_container.GetComponent<RectTransform>().rect.height; 

        if (containerHeight < contentHeight)
        {
            containerRect.offsetMin = new Vector2(containerRect.offsetMax.x, containerHeight - contentHeight);
        }
        else
        {
            if (contentHeight >= f_initialContainerHeight)
            {
                float difference = f_initialContainerHeight - contentHeight;
                float scrollBarValue = scrollBar.value;
                containerRect.offsetMax = new Vector2(containerRect.offsetMax.x, 0);
                containerRect.offsetMin = new Vector2(containerRect.offsetMax.x, difference);

                StartCoroutine(SetScrollBarValue(scrollBarValue));
            }
            else
            {
                containerRect.offsetMax = new Vector2(containerRect.offsetMax.x, 0f);
                containerRect.offsetMin = new Vector2(containerRect.offsetMax.x, 0f);
            }
        }
    }

    protected abstract float GetContentHeight();

    private IEnumerator SetScrollBarValue(float value)
    {
        yield return new WaitForSeconds(0.001f);
        scrollBar.value = value;
    }
}
