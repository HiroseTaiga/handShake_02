using UnityEngine;

public interface left_hand_tracking_source
{
    bool try_get_thumb_tip(out Vector2 position);
    bool try_get_index_tip(out Vector2 position);
    bool try_get_middle_tip(out Vector2 position);
    bool try_get_ring_tip(out Vector2 position);
    bool try_get_little_tip(out Vector2 position);
}
