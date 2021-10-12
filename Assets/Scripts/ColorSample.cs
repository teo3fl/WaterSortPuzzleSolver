using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSample : MonoBehaviour
{
    [SerializeField]
    private Image image;

    public void Delete()
    {
        Destroy(gameObject);
    }
}
