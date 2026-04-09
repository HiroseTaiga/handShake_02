using System;
using UnityEngine;

[Serializable]
public class puppet_balance_assist
{
    [SerializeField] private float tiltThreshold = 35f;
    [SerializeField] private float normalAngularDamping = 0.5f;
    [SerializeField] private float emergencyAngularDamping = 4f;
    [SerializeField] private bool boostHeadAssist = true;
    [SerializeField] private float normalHeadForceMultiplier = 1f;
    [SerializeField] private float emergencyHeadForceMultiplier = 1.75f;

    public void apply(puppet_parts parts, puppet_limb_handle headHandle)
    {
        if (parts == null || !parts.hasAnyPart())
        {
            if (headHandle != null)
            {
                headHandle.setForceMultiplier(normalHeadForceMultiplier);
            }

            return;
        }

        Rigidbody2D body = parts.bodyCore;

        if (body == null)
        {
            if (headHandle != null)
            {
                headHandle.setForceMultiplier(normalHeadForceMultiplier);
            }

            return;
        }

        float tilt = Mathf.Abs(Mathf.DeltaAngle(body.rotation, 0f));
        bool isTilted = tilt > tiltThreshold;

        body.angularDamping = isTilted ? emergencyAngularDamping : normalAngularDamping;

        if (headHandle == null)
        {
            return;
        }

        if (!boostHeadAssist)
        {
            headHandle.setForceMultiplier(normalHeadForceMultiplier);
            return;
        }

        headHandle.setForceMultiplier(isTilted ? emergencyHeadForceMultiplier : normalHeadForceMultiplier);
    }
}
