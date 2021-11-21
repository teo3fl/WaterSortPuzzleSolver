using System.Collections.Generic;

public class StateComparer : IComparer<State>
{
    public int Compare(State x, State y)
    {
        if (x.Value.Equals(y.Value))
            return 0;

        var result = x.Score.CompareTo(y.Score);
        if (result == 0)
            return 1;

        return result;
    }
}
