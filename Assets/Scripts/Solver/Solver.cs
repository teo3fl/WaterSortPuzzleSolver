using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    [SerializeField]
    private BeakerContainer beakerContainer;
    [SerializeField]
    private DialogHUD dialogHUD;


    public void Begin()
    {
        var data = beakerContainer.GetBeakers();
        string errorMessage = string.Empty;

        if (CheckData(data, ref errorMessage))
        {
            dialogHUD.Display("Calculating . . .", "Cancel", Cancel);
            StartCoroutine(Calculate(data));
        }
        else
        {
            dialogHUD.Display(errorMessage, "Ok");
        }
    }

    private bool CheckData(List<BeakerData> data, ref string errorMessage)
    {
        // check the min number of beakers

        if (data.Count < 2)
        {
            errorMessage = "The number of beakers can't be lesser than 2.";
            return false;
        }


        // content check


        var contentCounter = new Dictionary<int, int>();

        foreach (BeakerData beaker in data)
        {
            foreach (int id in beaker.contents)
            {
                if (id != 0)
                {
                    if (!contentCounter.ContainsKey(id))
                        contentCounter.Add(id, 0);
                    ++contentCounter[id];
                }
            }
        }

        // check if there are no colors at all

        if (contentCounter.Count < 2)
        {
            errorMessage = "The number of colors can't be lesser than 2.";
            return false;
        }

        // check if the number of colors is lesser or equal to the number of beakers
        if (contentCounter.Count > data.Count)
        {
            errorMessage = "The number of colors can't be gerater than the number of beakers.";
            return false;
        }

        // check if each color has maxCapacity apparitions
        int maxCapacity = Beaker.MaxCapacity;

        foreach (KeyValuePair<int, int> pair in contentCounter)
        {
            if (pair.Value != maxCapacity)
            {
                errorMessage = $"All colors must appear {maxCapacity} times.";
                return false;
            }
        }

        return true;
    }

    private void Cancel()
    {
        StopAllCoroutines();
    }

    private IEnumerator Calculate(List<BeakerData> data)
    {
        yield return 0;
    }
}
