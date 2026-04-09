using UnityEngine;

public class fingerDirectionMovementController : MonoBehaviour
{
    [Header("input condition")]
    [SerializeField] private handType targetHand;
    [SerializeField] private fingerType targetFinger;
    [SerializeField] private fingerDirection targetDirection;

    [Header("movement")]
    [SerializeField] private Rigidbody2D[] targetRigidbodies;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector3 movementAxis = Vector3.right;
    [SerializeField] private bool useMovePosition = false;

    [Header("dependency")]
    [SerializeField] private handInputDispatcher inputDispatcher;

    private bool isActiveDirection;

    private void OnEnable()
    {
        if (inputDispatcher != null)
        {
            inputDispatcher.onFingerDirectionChanged += onFingerDirectionChanged;
        }
    }

    private void OnDisable()
    {
        if (inputDispatcher != null)
        {
            inputDispatcher.onFingerDirectionChanged -= onFingerDirectionChanged;
        }
    }

    private void onFingerDirectionChanged(handType hand, fingerType finger, fingerDirection direction)
    {
        if (hand != targetHand) return;
        if (finger != targetFinger) return;

        if (direction == targetDirection)
        {
            isActiveDirection = true;
            return;
        }

        isActiveDirection = false;
    }

    private void FixedUpdate()
    {
        if (!isActiveDirection) return;
        if (targetRigidbodies == null) return;

        foreach (var rigidbody in targetRigidbodies)
        {
            if (rigidbody != null)
            {
                if (useMovePosition)
                {
                    rigidbody.MovePosition(rigidbody.position + (Vector2)(movementAxis * moveSpeed * Time.fixedDeltaTime));
                    continue;
                }
                rigidbody.linearVelocity = movementAxis * moveSpeed;
            }
        }
    }
}