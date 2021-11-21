using System;
using System.Linq;
using System.Collections.Generic;

public class State
{
    public State Parent { get; private set; }
    public Action Action { get; private set; }
    public List<Beaker> Beakers { get; private set; }

    public bool IsFinal { get; private set; }
    public int Score { get; private set; }

    public string Value { get; private set; }

    public State(List<Beaker> beakers)
    {
        CopyListContent(beakers);
        Parent = null;
        Action = null;

        CheckIfFinal();
        SetScore();

        UpdateValue();
    }

    public State(State parent, Action action)
    {
        CopyListContent(parent.Beakers);
        Action = action;
        Parent = parent;

        ProgressState();

        CheckIfFinal();
        SetScore();

        UpdateValue();
    }

    private void CopyListContent(List<Beaker> list)
    {
        Beakers = new List<Beaker>();
        foreach (Beaker beaker in list)
        {
            Beakers.Add(new Beaker(beaker));
        }
    }

    private void ProgressState()
    {
        var donnor = Beakers[Action.donnor];
        var recipient = Beakers[Action.recipient];
        
        Action.pouringCounter= donnor.PourInto(recipient);
    }

    private void CheckIfFinal()
    {
        foreach(var beaker in Beakers)
        {
            if(!beaker.IsSorted())
            {
                return;
            }
        }

        IsFinal = true;
    }

    private void SetScore()
    {
        if (IsFinal)
        {
            Score = int.MaxValue;
            return;
        }

        foreach(Beaker beaker in Beakers)
        {
            if(beaker.IsSorted())
            {
                Score += beaker.Contents.Count * 10; // 40 for a full beaker, 0 for an empty one
            }

            if(beaker.Contents.Count<2)
            {
                Score += (beaker.Contents.Count + 1) * 5;
            }
        }
    }

    public List<State> Expand()
    {
        var children = new List<State>();

        for (int i = 0; i < Beakers.Count; ++i)
        {
            for (int j = 0; j < Beakers.Count; ++j)
            {
                if (i != j && Beakers[i].CanPourInto(Beakers[j]))
                {
                    children.Add(new State(this, new Action() { donnor = i, recipient = j }));
                }
            }
        }

        return children;
    }

    public bool Contains(Beaker beaker)
    {
        foreach(var b in Beakers)
        {
            if (b.Equals(beaker))
                return true;
        }

        return false;
    }

    public void UpdateValue()
    {
        var beakerValues = (from beaker in Beakers
                         let code = beaker.Value
                         orderby code
                         select code).ToList();

        Value = string.Empty;

        foreach (var value in beakerValues)
        {
            Value += value.ToString() + '.';
        }

        Value = Value.TrimEnd('.');
    }
}
