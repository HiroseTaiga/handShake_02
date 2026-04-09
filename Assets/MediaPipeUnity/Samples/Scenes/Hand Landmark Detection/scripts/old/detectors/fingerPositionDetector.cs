using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public abstract class fingerPositionDetector : MonoBehaviour
{
    [SerializeField] protected HandLandmarkerRunner runner;
    [SerializeField] protected float threshold = 0.02f;

    protected virtual void Start()
    {
        runner.onHandResult += detect;
    }

    protected virtual void OnDestroy()
    {
        runner.onHandResult -= detect;
    }

    protected abstract void detect(HandLandmarkerResult result);
}