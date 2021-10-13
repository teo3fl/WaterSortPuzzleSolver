using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapacitySlider : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI txt_value;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        txt_value.text = slider.value.ToString();
    }

    public void OnValueChanged()
    {
        txt_value.text = slider.value.ToString();
        BeakerUI.MaxCapacity = (int)slider.value;
    }
}
