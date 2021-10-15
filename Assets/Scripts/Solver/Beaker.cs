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
        Contents = StackExtension.Clone(other.Contents);
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
                for (int i = 0; i < contents.Length-1; ++i)
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

        foreach (var content in Contents)
        {
            binary += Convert.ToString(content, 2).PadLeft(4, '0');
        }

        return Convert.ToInt32(binary, 2);
    }
}
