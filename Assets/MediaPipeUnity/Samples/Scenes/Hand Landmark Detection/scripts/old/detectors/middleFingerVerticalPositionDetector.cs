using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;

public class middleFingerVerticalPositionDetector : fingerPositionDetector
{
    public enum verticalState
    {
        up,
        center,
        down
    }

    public System.Action<verticalState> onStateChanged;

    protected override void detect(HandLandmarkerResult result)
    {
        if (result.handLandmarks == null || result.handLandmarks.Count == 0) return;

        var landmarks = result.handLandmarks[0].landmarks;

        var tip = landmarks[12];
        var mcp = landmarks[9];

        float delta = tip.y - mcp.y;

        verticalState state;

        if (Mathf.Abs(delta) < threshold)
            state = verticalState.center;
        else
            state = delta < 0 ? verticalState.up : verticalState.down;

        onStateChanged?.Invoke(state);
    }
}