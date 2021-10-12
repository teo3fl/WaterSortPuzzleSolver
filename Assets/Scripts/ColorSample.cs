using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSample : MonoBehaviour
{
    [SerializeField]
    private Image image;
    public Color Color 
    { 
        get{ return image.color; } 
        set { image.color = value; }
    }
   
    public ColorContainer container;


    public void Delete()
    {
        Destroy(gameObject);
    }

    public void ChangeColor()
    {
        container.RequestcolorChange(this);
    }
}
