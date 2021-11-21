using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerUI : MonoBehaviour
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
            updatedContentHeight = false;
        }
    }

    public static float ContentHeight { get; private set; }
    private static bool updatedContentHeight;


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
        if (!updatedContentHeight)
        {
            ContentHeight = (t_contents.GetComponent<RectTransform>().rect.height - t_addButton.GetComponent<RectTransform>().rect.height) / maxCapacity;
            updatedContentHeight = true;
        }
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
            t_contents.GetChild(i).GetComponent<BeakerContent>().Resize(ContentHeight);
        }
    }

    public void AddColor()
    {
        if (t_contents.childCount - 1 >= maxCapacity)
            return;

        var sample = Instantiate(go_contentSample, t_contents).GetComponent<BeakerContent>();

        sample.Initialize(ContentHeight, container.DefaultColorSample);
        sample.transform.SetSiblingIndex(1);

        t_addButton.SetSiblingIndex(0);
    }

    public void AddColor(ColorSample colorSample)
    {
        if (t_contents.childCount - 1 >= maxCapacity)
            return;

        var sample = Instantiate(go_contentSample, t_contents).GetComponent<BeakerContent>();

        sample.Initialize(ContentHeight, colorSample);
        sample.transform.SetSiblingIndex(1);

        t_addButton.SetSiblingIndex(0);
    }

    public void Delete()
    {
        container.DeleteElement(gameObject);
    }

    private void OnDestroy()
    {
        onCapacityChanged -= OnCapacitychanged;
    }

    public void Fill()
    {
        if (t_contents.childCount - 1 == maxCapacity)
            return;

        for (int i = t_contents.childCount - 1; i < maxCapacity; ++i)
        {
            AddColor();
        }
    }

    private void ResetContents()
    {
        for (int i = 1; i < t_contents.childCount; i++)
        {
            Destroy(t_contents.GetChild(i));
        }
    }

    public Beaker GetData()
    {
        var stack = new Stack<int>();
        for (int i = t_contents.childCount - 1; i > 0; --i)
        {
            stack.Push(t_contents.GetChild(i).GetComponent<BeakerContent>().ColorSource.ID);
        }
        return new Beaker(stack);
    }

    public void SetData(Beaker data)
    {
        ResetContents();

        foreach(int colorId in data.Contents.ToList())
        {
            AddColor(ColorContainer.Instance.GetSampleById(colorId));
        }

        OnCapacitychanged();
    }
}