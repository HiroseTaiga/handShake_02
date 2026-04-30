using System.Collections.Generic;

public class handState
{
    public handType handType { get; }
    private readonly Dictionary<fingerType, fingerState> fingers;

    public handState(handType handType, Dictionary<fingerType, fingerState> fingers)
    {
        this.handType = handType;
        this.fingers = fingers;
    }

    public IReadOnlyDictionary<fingerType, fingerState> fingerStates => fingers;

    public fingerState getFinger(fingerType finger)
    {
        return fingers[finger];
    }
}