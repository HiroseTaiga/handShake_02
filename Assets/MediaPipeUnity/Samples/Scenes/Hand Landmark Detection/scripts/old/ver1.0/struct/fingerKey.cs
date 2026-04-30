using System;

public struct fingerKey : IEquatable<fingerKey>
{
    public handType handType;
    public fingerType fingerType;

    public fingerKey(handType handType, fingerType fingerType)
    {
        this.handType = handType;
        this.fingerType = fingerType;
    }

    public bool Equals(fingerKey other)
    {
        return handType == other.handType && fingerType == other.fingerType;
    }

    public override bool Equals(object obj)
    {
        if (obj is fingerKey other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int)handType * 397) ^ (int)fingerType;
        }
    }
}