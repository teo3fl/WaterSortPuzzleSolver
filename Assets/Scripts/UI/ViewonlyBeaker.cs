using System.Collections.Generic;
using UnityEngine;

public class ViewonlyBeaker : MonoBehaviour
{
    [SerializeField]
    private Transform t_contents;
    public Transform t_Contents { get { return t_contents; } }
    [SerializeField]
    private GameObject go_contentSample;
    [SerializeField]
    private TMPro.TextMeshProUGUI txt_id;


    public void Initialize(int beakerId, Stack<int> contentIds)
    {
        foreach(int id in contentIds)
        {
            AddColor(id);
        }

        txt_id.text = beakerId.ToString();
    }

    private void AddColor(int sampleId)
    {
        var content = Instantiate(go_contentSample, t_Contents).GetComponent<ViewOnlyBeakerContent>();
        content.Initialize(sampleId);
    }

    public void PourInto(ViewonlyBeaker other)
    {
        var top = t_contents.GetChild(0);
        top.SetParent(other.t_Contents);
        top.SetAsFirstSibling();
    }
}
