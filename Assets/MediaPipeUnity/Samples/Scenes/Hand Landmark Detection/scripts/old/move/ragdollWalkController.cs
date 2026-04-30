using UnityEngine;

public class ragdollWalkController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int moveDirection = 1;
    [SerializeField] private bool isWalking;
    [SerializeField] private ragdollWalkMotion walkMotion = new ragdollWalkMotion();
    [SerializeField] private ragdollWalkParts parts = new ragdollWalkParts();

    private void OnValidate()
    {
        moveDirection = sanitizeDirection(moveDirection);
    }

    private void FixedUpdate()
    {
        moveDirection = sanitizeDirection(moveDirection);
        updateMovement();
        walkMotion.updateMotion(parts, isWalking, moveDirection, Time.fixedDeltaTime);
    }

    public void setWalking(bool value)
    {
        isWalking = value;
    }

    public void setMoveDirection(int direction)
    {
        moveDirection = sanitizeDirection(direction);
    }

    private void updateMovement()
    {
        Rigidbody2D bodyRigidbody = parts != null ? parts.body : null;

        if (bodyRigidbody == null)
        {
            return;
        }

        Vector2 velocity = bodyRigidbody.linearVelocity;
        velocity.x = isWalking ? moveDirection * moveSpeed : 0f;
        bodyRigidbody.linearVelocity = velocity;
    }

    private int sanitizeDirection(int direction)
    {
        return direction < 0 ? -1 : 1;
    }
}
