using System.Collections.Generic;

public class fingerDirectionStateStore
{
    private readonly Dictionary<fingerKey, fingerDirection> states = new Dictionary<fingerKey, fingerDirection>();

    public bool tryUpdate(handType hand, fingerType finger, fingerDirection direction)
    {
        fingerKey key = new fingerKey(hand, finger);

        if (states.TryGetValue(key, out var prev))
        {
            if (prev == direction)
            {
                return false;
            }
        }

        states[key] = direction;
        return true;
    }

    public void removeUnusedHands(HashSet<handType> activeHands)
    {
        var removeList = new List<fingerKey>();

        foreach (var kv in states)
        {
            if (!activeHands.Contains(kv.Key.handType))
            {
                removeList.Add(kv.Key);
            }
        }

        foreach (var k in removeList)
        {
            states.Remove(k);
        }
    }
}