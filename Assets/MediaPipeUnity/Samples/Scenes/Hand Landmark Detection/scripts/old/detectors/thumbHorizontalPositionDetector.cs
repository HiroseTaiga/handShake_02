using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;

public class thumbHorizontalPositionDetector : fingerPositionDetector
{
    public enum horizontalState
    {
        left,
        center,
        right
    }

    public System.Action<horizontalState> onStateChanged;

    protected override void detect(HandLandmarkerResult result)
    {
        if (result.handLandmarks == null || result.handLandmarks.Count == 0) return;

        var landmarks = result.handLandmarks[0].landmarks;

        var thumbTip = landmarks[4];
        var indexMcp = landmarks[5];

        float delta = thumbTip.x - indexMcp.x;

        horizontalState state;

        if (Mathf.Abs(delta) < threshold)
            state = horizontalState.center;
        else
            state = delta > 0 ? horizontalState.right : horizontalState.left;

        onStateChanged?.Invoke(state);
    }
}