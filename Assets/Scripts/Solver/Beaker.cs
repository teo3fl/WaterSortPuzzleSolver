using System;
using System.Collections.Generic;

public class Beaker
{
    public Stack<int> Contents { get; private set; }
    public static int maxCapacity;

    public Beaker(Stack<int> contents)
    {
        Contents = contents;
    }

    public Beaker(Beaker other)
    {
        Contents = new Stack<int>(other.Contents);
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

        return Contents.Count > 0 && (other.Contents.Count == 0 || (other.Contents.Peek() == Contents.Peek()));
    }

    public void PourInto(Beaker other)
    {
        if (!CanPourInto(other))
        {
            throw new System.Exception("Beaker.PourInto(): invalid action.");
        }

        other.Contents.Push(Contents.Pop());
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

        var contents = Contents.ToArray();
        var otherContents = (obj as Beaker).Contents.ToArray();

        if (contents.Length != otherContents.Length)
            return false;

        for (int i = 0; i < contents.Length; ++i)
        {
            if (contents[i] != otherContents[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        // the ID of each content will be converted to binary (4 bits), then concatenated, then the result will be converted to base 10

        string binary = string.Empty;

        foreach(var content in Contents)
        {
            binary += Convert.ToString(content, 2).PadLeft(4, '0');
        }

        return Convert.ToInt32(binary, 2);
    }
}