using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class thumbController : MonoBehaviour
{
    private enum handType
    {
        right,
        left
    }

    [SerializeField] private HandLandmarkerRunner runner;
    [SerializeField] private Rigidbody2D[] rbs;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float threshold = 0.02f;
    [SerializeField] private handType controlHand = handType.right;

    private int moveDirection;

    private void Start()
    {
        runner.onHandResult += applyHandResult;
    }

    private void OnDestroy()
    {
        runner.onHandResult -= applyHandResult;
    }

    public void applyHandResult(HandLandmarkerResult result)
    {
        if (result.handLandmarks == null || result.handLandmarks.Count == 0)
        {
            moveDirection = 0;
            return;
        }

        string targetHand = controlHand == handType.right ? "Right" : "Left";
        int targetIndex = -1;

        for (int i = 0; i < result.handedness.Count; i++)
        {
            string handedness = result.handedness[i].categories[0].categoryName;

            if (handedness == targetHand)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1)
        {
            moveDirection = 0;
            return;
        }

        var landmarks = result.handLandmarks[targetIndex];

        var thumbTip = landmarks.landmarks[4];
        var indexMcp = landmarks.landmarks[5];

        float deltaX = thumbTip.x - indexMcp.x;

        if (Mathf.Abs(deltaX) < threshold)
        {
            moveDirection = 0;
            return;
        }

        if (controlHand == handType.right)
        {
            moveDirection = deltaX < 0 ? -1 : 1;
        }
        else
        {
            moveDirection = deltaX > 0 ? -1 : 1;
        }
    }

    private void Update()
    {
        if (rbs == null || rbs.Length == 0) return;
        if (moveDirection == 0) return;

        foreach (var rb in rbs)
        {
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
        }
    }
}