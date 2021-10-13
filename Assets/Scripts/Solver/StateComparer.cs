using System.Collections.Generic;

public class StateComparer : IComparer<State>
{
    public int Compare(State x, State y)
    {
        return x.Score.CompareTo(y.Score);
    }
}
