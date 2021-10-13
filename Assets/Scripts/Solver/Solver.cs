using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    [SerializeField]
    private BeakerContainer beakerContainer;

    public void Begin()
    {
        var data = beakerContainer.GetBeakers();

        if(CheckData(data))
        {
            // start calculating
        }
        else
        {
            // display message (might turn this into an enum instead of bool so that it can display the exact problem)
        }
    }

    private bool CheckData(List<BeakerData> data)
    {
        return false;
    }
}
