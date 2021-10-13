using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaker : MonoBehaviour
{
    [SerializeField]
    private Transform t_contents;
    [SerializeField]
    private Transform t_addButton;
    [SerializeField]
    private GameObject go_contentSample;

    public BeakerContainer container;


    private delegate void CapacityChangeHandler();
    private static CapacityChangeHandler onCapacityChanged;

    private static int maxCapacity = 2;
    public static int MaxCapacity
    {
        get { return maxCapacity; }
        set
        {
            maxCapacity = value;
            onCapacityChanged?.Invoke();
        }
    }

    private float contentHeight;


    public void Initialize()
    {
        onCapacityChanged += OnCapacitychanged;
        StartCoroutine(SetContentHeight());
    }

    private IEnumerator SetContentHeight()
    {
        yield return new WaitForSeconds(0.001f);
        UpdatecontentHeight();
    }

    private void UpdatecontentHeight()
    {
        contentHeight = (t_contents.GetComponent<RectTransform>().rect.height - t_addButton.GetComponent<RectTransform>().rect.height) / maxCapacity;
    }

    private void OnCapacitychanged()
    {
        StopAllCoroutines();
        StartCoroutine(Updatecontent());
    }

    private IEnumerator Updatecontent()
    {
        DeleteSurplusContent();
        yield return new WaitForSeconds(0.001f);
        UpdatecontentHeight();
        ResizeContent();
    }

    private void DeleteSurplusContent()
    {
        if (t_contents.childCount - 1 > maxCapacity)
        {
            for (int i = 1; i < t_contents.childCount - maxCapacity; ++i)
            {
                Destroy(t_contents.GetChild(i).gameObject);
            }
        }
    }

    private void ResizeContent()
    {
        for (int i = 1; i < t_contents.childCount; ++i)
        {
            t_contents.GetChild(i).GetComponent<BeakerContent>().Resize(contentHeight);
        }
    }

    public void AddColor()
    {
        if (t_contents.childCount - 1 >= maxCapacity)
            return;

        var sample = Instantiate(go_contentSample, t_contents).GetComponent<BeakerContent>();

        sample.Initialize(contentHeight, container.DefaultColorSample);
        sample.transform.SetSiblingIndex(1);

        t_addButton.SetSiblingIndex(0);
    }

    public void Delete()
    {
        onCapacityChanged -= OnCapacitychanged;
        container.DeleteElement(gameObject);
    }

    public BeakerData GetData()
    {
        var stack = new Stack<int>();
        for(int i = t_contents.childCount-1; i>0;--i)
        {
            stack.Push(t_contents.GetChild(i).GetComponent<BeakerContent>().ColorSource.ID);
        }

        while(stack.Count < maxCapacity)
        {
            stack.Push(0);
        }
        return new BeakerData() { contents = stack };
    }
}