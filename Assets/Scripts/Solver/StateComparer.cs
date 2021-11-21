using System.Collections.Generic;

public class StateComparer : IComparer<State>
{
    public int Compare(State x, State y)
    {
        var result = x.Score.CompareTo(y.Score);
        if (result == 0)
            return 1;

        return result;
    }
}
