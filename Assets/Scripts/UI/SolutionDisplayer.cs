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
    [SerializeField]
    private TMPro.TextMeshProUGUI txt_currentStep;


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

    private List<Action> steps;
    bool movedForward;


    public void Initialize(State initialState, List<Action> actions)
    {
        int idCounter = 1;
        beakers = new List<ViewonlyBeaker>();
        foreach (Beaker beaker in initialState.Beakers)
        {
            var b = Instantiate(go_beaker, t_contents).GetComponent<ViewonlyBeaker>();
            b.Initialize(idCounter++, beaker.Contents);
            beakers.Add(b);
        }

        steps = actions;
        movedForward = true;
        CurrentStep = 0;
    }

    private void Clear()
    {
        for (int i = 0; i < beakers.Count; ++i)
        {
            Destroy(beakers[i].gameObject);
        }

        beakers = null;
    }

    public void OnNextButtonclicked()
    {
        movedForward = true;
        ++CurrentStep;
    }

    public void OnPreviousButtonClicked()
    {
        movedForward = false;
        --CurrentStep;
    }

    private void UpdateBeakers()
    {
        int actionIndex = movedForward ? CurrentStep - 1 : CurrentStep;

        var action = steps[actionIndex];
        var firstBeaker = beakers[movedForward ? action.donnor : action.recipient];
        var secondBeaker = beakers[movedForward ? action.recipient : action.donnor];

        for (int i = 0; i < action.pouringCounter; ++i)
            firstBeaker.PourInto(secondBeaker);
    }

    private void UpdateUI()
    {
        txt_currentStep.text = $"{CurrentStep + 1} / {steps.Count + 1}";

        if (CurrentStep == 0)
        {
            btn_previous.SetActive(false);
            btn_next.SetActive(true);
        }
        else if (CurrentStep == 1)
        {
            btn_previous.SetActive(true);
        }

        if (CurrentStep == steps.Count)
        {
            btn_next.SetActive(false);
        }
        else if (CurrentStep == steps.Count - 1)
        {
            btn_next.SetActive(true);
        }

        if (currentStep == steps.Count)
        {
            txt_stepDescription.text = string.Empty; // allow the very last step to be displayed simply for seeing the result of the last step 
            return;
        }

        var firstBeaker = steps[currentStep].donnor + 1;
        var secondBeaker = steps[currentStep].recipient + 1;

        txt_stepDescription.text = $"Pour {firstBeaker} into {secondBeaker}";
    }

    public void OnBackButtonPressed()
    {
        Clear();
    }
}
