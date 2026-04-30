using System;
using UnityEngine;

[Serializable]
public class puppet_limb_handle
{
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private Rigidbody2D controlledBody;
    [SerializeField] private Rigidbody2D targetBody;
    [SerializeField] private DistanceJoint2D targetJoint;
    //[SerializeField] private float frequency = 6f;
    //[SerializeField] private float dampingRatio = 0.8f;

    public void initialize()
    {
        applySettings();
    }

    public void setEnabled(bool value)
    {
        isEnabled = value;
        updateJointEnabledState(isEnabled && targetBody != null);
    }

    public void setForceMultiplier(float value)
    {
    }

    public void updateHandle(bool hasTarget)
    {
        if (targetJoint == null || controlledBody == null || targetBody == null)
        {
            return;
        }

        bool canDrive = isEnabled && hasTarget;
        updateJointEnabledState(canDrive);

        if (!canDrive)
        {
            return;
        }

        applySettings();
    }

    private void applySettings()
    {
        if (targetJoint == null)
        {
            return;
        }

        targetJoint.connectedBody = targetBody;
        targetJoint.autoConfigureDistance = false;
        //targetJoint.distance = 0f;
        //targetJoint.frequency = frequency;
        //targetJoint.dampingRatio = dampingRatio;
        //targetJoint.maxDistanceOnly = false;
    }

    private void updateJointEnabledState(bool value)
    {
        if (targetJoint == null)
        {
            return;
        }

        targetJoint.enabled = value;
    }
}
