using System.Collections.Generic;

public static class StackExtension
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
}
