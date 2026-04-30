using UnityEngine;

public class simulated_left_hand_tracking_source : MonoBehaviour, left_hand_tracking_source
{
    [SerializeField] private Transform thumbTip;
    [SerializeField] private Transform indexTip;
    [SerializeField] private Transform middleTip;
    [SerializeField] private Transform ringTip;
    [SerializeField] private Transform littleTip;

    public bool try_get_thumb_tip(out Vector2 position)
    {
        return try_get_position(thumbTip, out position);
    }

    public bool try_get_index_tip(out Vector2 position)
    {
        return try_get_position(indexTip, out position);
    }

    public bool try_get_middle_tip(out Vector2 position)
    {
        return try_get_position(middleTip, out position);
    }

    public bool try_get_ring_tip(out Vector2 position)
    {
        return try_get_position(ringTip, out position);
    }

    public bool try_get_little_tip(out Vector2 position)
    {
        return try_get_position(littleTip, out position);
    }

    private bool try_get_position(Transform target, out Vector2 position)
    {
        if (target == null)
        {
            position = default;
            return false;
        }

        position = target.position;
        return true;
    }
}
