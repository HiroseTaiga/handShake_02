using System;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine;

public class hand_landmarker_fingertip_follower : MonoBehaviour
{
    private struct normalized_tip_position
    {
        public bool isValid;
        public float x;
        public float y;
        public float z;
    }

    private enum tracked_hand
    {
        left,
        right
    }

    [Serializable]
    private class fingertip_target
    {
        [SerializeField] private Transform target;

        private Rigidbody2D cachedRigidbody;
        private bool hasPendingPosition;
        private Vector3 pendingWorldPosition;

        public void initialize()
        {
            cachedRigidbody = target != null ? target.GetComponent<Rigidbody2D>() : null;
        }

        public void setTargetPosition(Vector3 worldPosition)
        {
            pendingWorldPosition = worldPosition;
            hasPendingPosition = true;
        }

        public void applyPendingPosition()
        {
            if (!hasPendingPosition || target == null)
            {
                return;
            }

            if (cachedRigidbody != null)
            {
                if (cachedRigidbody.bodyType == RigidbodyType2D.Kinematic)
                {
                    cachedRigidbody.position = (Vector2)pendingWorldPosition;
                }

                return;
            }

            target.position = pendingWorldPosition;
        }

        public Vector3 getPosition()
        {
            return target.position;
        }
    }

    [SerializeField] private HandLandmarkerRunner runner;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private tracked_hand targetHand = tracked_hand.left;
    //[SerializeField] private float worldDepth = 10f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY = true;
    [SerializeField] private Vector3 worldOffset;
    [SerializeField] private fingertip_target thumb = new fingertip_target();
    [SerializeField] private fingertip_target index = new fingertip_target();
    [SerializeField] private fingertip_target middle = new fingertip_target();
    [SerializeField] private fingertip_target ring = new fingertip_target();
    [SerializeField] private fingertip_target little = new fingertip_target();

    private readonly object resultLock = new object();
    private normalized_tip_position thumbTip;
    private normalized_tip_position indexTip;
    private normalized_tip_position middleTip;
    private normalized_tip_position ringTip;
    private normalized_tip_position littleTip;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        thumb.initialize();
        index.initialize();
        middle.initialize();
        ring.initialize();
        little.initialize();
    }

    private void OnEnable()
    {
        if (runner != null)
        {
            runner.onHandResult += onHandResult;
        }
    }

    private void OnDisable()
    {
        if (runner != null)
        {
            runner.onHandResult -= onHandResult;
        }
    }

    private void FixedUpdate()
    {
        normalized_tip_position thumbSnapshot;
        normalized_tip_position indexSnapshot;
        normalized_tip_position middleSnapshot;
        normalized_tip_position ringSnapshot;
        normalized_tip_position littleSnapshot;

        lock (resultLock)
        {
            thumbSnapshot = thumbTip;
            indexSnapshot = indexTip;
            middleSnapshot = middleTip;
            ringSnapshot = ringTip;
            littleSnapshot = littleTip;
        }

        applyTipToTarget(thumb, thumbSnapshot);
        applyTipToTarget(index, indexSnapshot);
        applyTipToTarget(middle, middleSnapshot);
        applyTipToTarget(ring, ringSnapshot);
        applyTipToTarget(little, littleSnapshot);

        thumb.applyPendingPosition();
        index.applyPendingPosition();
        middle.applyPendingPosition();
        ring.applyPendingPosition();
        little.applyPendingPosition();
    }

    private void onHandResult(HandLandmarkerResult result)
    {
        if (result.handLandmarks == null || result.handedness == null)
        {
            return;
        }

        int handIndex = findHandIndex(result);

        if (handIndex < 0)
        {
            return;
        }

        var landmarks = result.handLandmarks[handIndex].landmarks;

        if (landmarks == null || landmarks.Count < 21)
        {
            return;
        }

        lock (resultLock)
        {
            thumbTip = createNormalizedTip(landmarks[4].x, landmarks[4].y, landmarks[4].z);
            indexTip = createNormalizedTip(landmarks[8].x, landmarks[8].y, landmarks[8].z);
            middleTip = createNormalizedTip(landmarks[12].x, landmarks[12].y, landmarks[12].z);
            ringTip = createNormalizedTip(landmarks[16].x, landmarks[16].y, landmarks[16].z);
            littleTip = createNormalizedTip(landmarks[20].x, landmarks[20].y, landmarks[20].z);
        }
    }

    private int findHandIndex(HandLandmarkerResult result)
    {
        if (result.handLandmarks == null || result.handedness == null)
        {
            return -1;
        }

        string targetHandName = targetHand == tracked_hand.left ? "Left" : "Right";

        for (int i = 0; i < result.handLandmarks.Count; i++)
        {
            if (i >= result.handedness.Count)
            {
                break;
            }

            var classifications = result.handedness[i];

            if (classifications.categories == null || classifications.categories.Count == 0)
            {
                continue;
            }

            if (classifications.categories[0].categoryName == targetHandName)
            {
                return i;
            }
        }

        return -1;
    }

    private Vector3 toWorldPosition(float normalizedX, float normalizedY, float normalizedZ)
    {
        float viewportX = invertX ? 1f - normalizedX : normalizedX;
        float viewportY = invertY ? 1f - normalizedY : normalizedY;
        Vector3 viewportPoint = new Vector3(viewportX, viewportY, transform.position.z);
        Vector3 worldPosition = targetCamera.ViewportToWorldPoint(viewportPoint);
        worldPosition.x += worldOffset.x;
        worldPosition.y += worldOffset.y;
        worldPosition.z = transform.position.z;
        return worldPosition;
    }

    private void applyTipToTarget(fingertip_target target, normalized_tip_position tip)
    {
        if (!tip.isValid || targetCamera == null)
        {
            return;
        }

        target.setTargetPosition(toWorldPosition(tip.x, tip.y, tip.z));
    }

    private normalized_tip_position createNormalizedTip(float x, float y, float z)
    {
        return new normalized_tip_position
        {
            isValid = true,
            x = x,
            y = y,
            z = z,
        };
    }
}
