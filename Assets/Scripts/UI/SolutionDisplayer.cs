using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject go_beaker;
    [SerializeField]
    private Transform t_contents;
    [SerializeField]
    private GameObject btn_next;
    [SerializeField]
    private GameObject btn_previous;
    [SerializeField]
    private TMPro.TextMeshProUGUI txt_stepDescription;


    private List<ViewonlyBeaker> beakers;

    private int currentStep;
    private int CurrentStep
    {
        get { return currentStep; }
        set
        {
            currentStep = value;
            if (value > 0 || !movedForward)
                UpdateBeakers();
            UpdateUI();
        }
    }

    private List<Tuple<int, int>> steps;
    bool movedForward;


    public void Initialize(State initialState, List<Tuple<int, int>> actions)
    {
        int idCounter = 1;
        foreach (Beaker beaker in initialState.Beakers)
        {
            var b = Instantiate(go_beaker, t_contents).GetComponent<ViewonlyBeaker>();
            b.Initialize(idCounter++, beaker.Contents);
        }

        steps = actions;
        CurrentStep = 0;
        movedForward = true;
    }

    public void OnNextButtonclicked()
    {
        ++CurrentStep;
        movedForward = true;
    }

    public void OnPreviousButtonClicked()
    {
        --CurrentStep;
        movedForward = false;
    }

    private void UpdateBeakers()
    {
        var action = steps[CurrentStep- 1];
        var firstBeaker = beakers[action.Item1];
        var secondBeaker = beakers[action.Item2];

        if (movedForward)
        {
            firstBeaker.PourInto(secondBeaker);
        }
        else
        {
            secondBeaker.PourInto(firstBeaker);
        }
    }

    private void UpdateUI()
    {
        if (CurrentStep == 0)
        {
            btn_previous.SetActive(false);
        }
        else if (CurrentStep == 1)
        {
            btn_previous.SetActive(true);
        }

        if (CurrentStep == steps.Count - 1)
        {
            btn_next.SetActive(false);
        }
        else if (CurrentStep == steps.Count - 2)
        {
            btn_next.SetActive(true);
        }

        var firstBeaker = steps[currentStep].Item1;
        var secondBeaker = steps[currentStep].Item2;

        txt_stepDescription.text = $"Pour {firstBeaker} into {secondBeaker}";
    }
}
