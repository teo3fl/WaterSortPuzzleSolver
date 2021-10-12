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

    // container

    public void AddColor()
    {
        var sample = Instantiate(go_contentSample, t_contents).GetComponent<BeakerContent>();

        sample.beaker = this;
        sample.transform.SetSiblingIndex(1);

        t_addButton.SetSiblingIndex(0);
    }

    public void DeleteColor(GameObject color)
    {
        Destroy(color);
    }
}