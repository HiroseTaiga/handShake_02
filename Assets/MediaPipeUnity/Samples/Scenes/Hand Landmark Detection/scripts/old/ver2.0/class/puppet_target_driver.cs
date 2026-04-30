using System;
using UnityEngine;

[Serializable]
public class puppet_target_driver
{
    [SerializeField] private MonoBehaviour trackingSourceBehaviour;
    [SerializeField] private float followSharpness = 20f;
    [SerializeField] private float worldScale = 1f;
    [SerializeField] private Vector2 worldOffset;
    [SerializeField] private bool useClamp;
    [SerializeField] private Vector2 minWorldPosition = new Vector2(-5f, -5f);
    [SerializeField] private Vector2 maxWorldPosition = new Vector2(5f, 5f);
    [SerializeField] private bool keepLastPositionWhenMissing = true;
    [SerializeField] private bool deactivateTargetWhenMissing;

    [NonSerialized] private left_hand_tracking_source trackingSource;
    [NonSerialized] private target_state thumbState = new target_state();
    [NonSerialized] private target_state indexState = new target_state();
    [NonSerialized] private target_state middleState = new target_state();
    [NonSerialized] private target_state ringState = new target_state();
    [NonSerialized] private target_state littleState = new target_state();

    public void initialize()
    {
        trackingSource = trackingSourceBehaviour as left_hand_tracking_source;
    }

    public void updateTargets(finger_target_set targetSet, float deltaTime)
    {
        if (targetSet == null || !targetSet.hasAnyTarget())
        {
            return;
        }

        if (trackingSource == null)
        {
            initialize();
        }

        updateTarget(targetSet.thumb, thumbState, tryGetThumb, deltaTime);
        updateTarget(targetSet.index, indexState, tryGetIndex, deltaTime);
        updateTarget(targetSet.middle, middleState, tryGetMiddle, deltaTime);
        updateTarget(targetSet.ring, ringState, tryGetRing, deltaTime);
        updateTarget(targetSet.little, littleState, tryGetLittle, deltaTime);
    }

    public bool is_left_hand_target_active()
    {
        return indexState.isActive;
    }

    public bool is_right_hand_target_active()
    {
        return ringState.isActive;
    }

    public bool is_left_foot_target_active()
    {
        return thumbState.isActive;
    }

    public bool is_right_foot_target_active()
    {
        return littleState.isActive;
    }

    public bool is_head_target_active()
    {
        return middleState.isActive;
    }

    private void updateTarget(Rigidbody2D targetBody, target_state state, tip_getter getter, float deltaTime)
    {
        if (targetBody == null)
        {
            state.isActive = false;
            return;
        }

        Vector2 tipPosition;
        bool hasTip = getter(out tipPosition);

        if (hasTip)
        {
            Vector2 targetPosition = convertPosition(tipPosition);
            Vector2 nextPosition = Vector2.Lerp(targetBody.position, targetPosition, getLerpFactor(deltaTime));

            targetBody.MovePosition(nextPosition);
            state.lastPosition = targetPosition;
            state.hasLastPosition = true;
            state.isActive = true;
            return;
        }

        if (keepLastPositionWhenMissing && state.hasLastPosition)
        {
            Vector2 nextPosition = Vector2.Lerp(targetBody.position, state.lastPosition, getLerpFactor(deltaTime));
            targetBody.MovePosition(nextPosition);
            state.isActive = !deactivateTargetWhenMissing;
            return;
        }

        state.isActive = false;
    }

    private Vector2 convertPosition(Vector2 sourcePosition)
    {
        Vector2 scaledPosition = sourcePosition * worldScale + worldOffset;

        if (!useClamp)
        {
            return scaledPosition;
        }

        float x = Mathf.Clamp(scaledPosition.x, minWorldPosition.x, maxWorldPosition.x);
        float y = Mathf.Clamp(scaledPosition.y, minWorldPosition.y, maxWorldPosition.y);
        return new Vector2(x, y);
    }

    private float getLerpFactor(float deltaTime)
    {
        if (followSharpness <= 0f)
        {
            return 1f;
        }

        return 1f - Mathf.Exp(-followSharpness * deltaTime);
    }

    private bool tryGetThumb(out Vector2 position)
    {
        if (trackingSource == null)
        {
            position = default;
            return false;
        }

        return trackingSource.try_get_thumb_tip(out position);
    }

    private bool tryGetIndex(out Vector2 position)
    {
        if (trackingSource == null)
        {
            position = default;
            return false;
        }

        return trackingSource.try_get_index_tip(out position);
    }

    private bool tryGetMiddle(out Vector2 position)
    {
        if (trackingSource == null)
        {
            position = default;
            return false;
        }

        return trackingSource.try_get_middle_tip(out position);
    }

    private bool tryGetRing(out Vector2 position)
    {
        if (trackingSource == null)
        {
            position = default;
            return false;
        }

        return trackingSource.try_get_ring_tip(out position);
    }

    private bool tryGetLittle(out Vector2 position)
    {
        if (trackingSource == null)
        {
            position = default;
            return false;
        }

        return trackingSource.try_get_little_tip(out position);
    }

    private delegate bool tip_getter(out Vector2 position);

    [Serializable]
    private class target_state
    {
        public bool hasLastPosition;
        public bool isActive;
        public Vector2 lastPosition;
    }
}
