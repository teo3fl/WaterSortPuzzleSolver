using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    [SerializeField]
    private BeakerContainer beakerContainer;
    [SerializeField]
    private DialogHUD dialogHUD;
    [SerializeField]
    private GameObject go_solutionDisplayer;
    [SerializeField]
    private GameObject go_initializer;


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

    private bool CheckData(List<Beaker> data, ref string errorMessage)
    {
        // check the min number of beakers

        if (data.Count < 2)
        {
            errorMessage = "The number of beakers can't be lesser than 2.";
            return false;
        }


        // content check


        var contentCounter = new Dictionary<int, int>();

        foreach (Beaker beaker in data)
        {
            foreach (int id in beaker.Contents)
            {
                if (!contentCounter.ContainsKey(id))
                    contentCounter.Add(id, 0);
                ++contentCounter[id];

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
        int maxCapacity = BeakerUI.MaxCapacity;

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

    private IEnumerator Calculate(List<Beaker> data)
    {
        Beaker.maxCapacity = BeakerUI.MaxCapacity;

        var opened = new SortedSet<State>(new StateComparer());
        var closed = new List<State>();

        opened.Add(new State(data));


        while (opened.Count > 0 && !opened.Max.IsFinal)
        {
            var currentState = opened.Max;
            opened.Remove(currentState);

            var children = currentState.Expand();
            foreach (var child in children)
            {
                if (!closed.Contains(child))
                    opened.Add(child);
            }

            closed.Add(currentState);

            yield return 0;
        }

        if (opened.Max != null)
            HandleSolution(closed[0], opened.Max);
        else
            HandleFailure();
    }

    private void HandleFailure()
    {

    }

    private void HandleSolution(State initial, State final)
    {
        dialogHUD.Display("Success", "Ok");
        go_initializer.SetActive(false);
        go_solutionDisplayer.SetActive(true);
        go_solutionDisplayer.GetComponent<SolutionDisplayer>().Initialize(initial, GetActions(initial,final));
    }

    private List<Tuple<int, int>> GetActions(State initial,State final)
    {
        var list = new List<Tuple<int, int>>();

        var currentState = final;

        while(currentState!=initial)
        {
            list.Add(currentState.Action);
            currentState = currentState.Parent;
        }

        list.Reverse();

        return list;
    }
}
