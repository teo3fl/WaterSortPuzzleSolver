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

    public State(List<Beaker> beakers)
    {
        CopyListContent(beakers);
        Parent = null;
        Action = null;

        CheckIfFinal();
        SetScore();
    }

    public State(State parent, Action action)
    {
        CopyListContent(parent.Beakers);
        Action = action;
        Parent = parent;

        ProgressState();

        CheckIfFinal();
        SetScore();
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

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = obj as State;

        foreach (var beaker in Beakers)
        {
            if (!other.Contains(beaker))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var hashCodes = (from beaker in Beakers
                         let code = beaker.GetHashCode()
                         orderby code
                         select code).ToList();

        string binary = string.Empty;

        foreach (var code in hashCodes)
        {
            binary += Convert.ToString(code, 2).PadLeft(4, '0');
        }

        return Convert.ToInt32(binary, 2);
    }
}
