using System;
using System.Collections.Generic;

[Serializable]
public class Beaker
{
    public Stack<int> Contents { get; private set; }
    public static int maxCapacity;

    public string Value { get; private set; } // used for checking equality


    public Beaker(Stack<int> contents)
    {
        Contents = contents;
        UpdateValue();
    }

    public Beaker(Beaker other)
    {
        Contents = other.Contents.Clone();
        Value = other.Value;
    }

    public void UpdateValue()
    {
        Value = string.Empty;

        var contentsArray = Contents.ToArray();
        for (int i = 0; i < contentsArray.Length; i++)
        {
            Value += contentsArray[i].ToString() + '.';
        }

        if (contentsArray.Length < maxCapacity)
        {
            for (int i = contentsArray.Length; i < maxCapacity; i++)
            {
                Value += "0.";
            }
        }
        Value = Value.TrimEnd('.');
    }

    public bool IsSorted()
    {
        if (Contents.Count == 0)
            return true;

        if (Contents.Count == maxCapacity)
        {
            int[] contents = Contents.ToArray();

            for (int i = 0; i < contents.Length - 1; ++i)
            {
                if (contents[i] != contents[i + 1])
                    return false;
            }

            return true;
        }

        return false;
    }

    public bool CanPourInto(Beaker other) // returns true if this beaker can pour the top content onto the other beaker
    {
        // can pour only if this is not empty AND ( other is empty OR ( has the same color on top && capacity != maxSize)
        bool isValidAction = Contents.Count > 0 && (other.Contents.Count == 0 || (other.Contents.Peek() == Contents.Peek()) && other.Contents.Count < maxCapacity);

        if (isValidAction)
        {
            // if the contents of this beaker have the same color && are pouring into an empty one, then there is no point
            if (other.Contents.Count == 0)
            {
                if (Contents.Count == 1)
                    return false; // empty recipient and a single content in this one

                var contents = Contents.ToArray();
                for (int i = 0; i < contents.Length - 1; ++i)
                {
                    if (contents[i] != contents[i + 1])
                        return true; // empty recipient but different contents contained in this one
                }

                return false; // empty recipient and same contents in this one
            }

            return true; // valid action, both beakers have contents
        }

        return false; // invalid action
    }

    public int PourInto(Beaker other)
    {
        if (!CanPourInto(other))
        {
            throw new System.Exception("Beaker.PourInto(): invalid action.");
        }

        int pouringCounter = 0;
        while (CanPourInto(other))
        {
            other.Contents.Push(Contents.Pop());
            ++pouringCounter;
        }

        UpdateValue();
        other.UpdateValue();

        return pouringCounter;
    }
}
