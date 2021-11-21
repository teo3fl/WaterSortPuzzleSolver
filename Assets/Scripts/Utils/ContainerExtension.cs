using System.Collections.Generic;
using System.Linq;

public static class ContainerExtension
{
    public static Stack<T> Clone<T>(this Stack<T> stack)
    {
        T[] array = stack.ToArray();
        var newStack = new Stack<T>();

        for (int i = array.Length - 1; i >= 0; --i)
        {
            newStack.Push(array[i]);
        }

        return newStack;
    }

    public static List<T> ToList<T>(this Stack<T> stack)
    {
        var list = new List<T>(stack);
        list.Reverse();
        return list;
    }

    public static bool ContainsValue(this List<State> list, State obj)
    {
        var value = obj.Value;

        foreach(var item in list)
        {
            if (item.Value.Equals(value))
                return true;
        }

        return false;
    }
}
