using System;
using UnityEngine;

public class handInputDispatcher : MonoBehaviour
{
    public event Action<handType, fingerType, fingerDirection> onFingerDirectionChanged;
    public event Action<handState> onHandStateUpdated;

    public void dispatchFinger(handType hand, fingerType finger, fingerDirection direction)
    {
        onFingerDirectionChanged?.Invoke(hand, finger, direction);
    }

    public void dispatchHand(handState state)
    {
        onHandStateUpdated?.Invoke(state);
    }
}