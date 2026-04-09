using System;
using UnityEngine;

[Serializable]
public class ragdollWalkParts
{
    [SerializeField] private Rigidbody2D bodyRigidbody;
    [SerializeField] private HingeJoint2D bodyJoint;
    [SerializeField] private HingeJoint2D leftArmJoint;
    [SerializeField] private HingeJoint2D rightArmJoint;
    [SerializeField] private HingeJoint2D leftLegJoint;
    [SerializeField] private HingeJoint2D rightLegJoint;

    public Rigidbody2D body => bodyRigidbody;
    public HingeJoint2D bodyHinge => bodyJoint;
    public HingeJoint2D leftArm => leftArmJoint;
    public HingeJoint2D rightArm => rightArmJoint;
    public HingeJoint2D leftLeg => leftLegJoint;
    public HingeJoint2D rightLeg => rightLegJoint;

    public bool hasAnyPart()
    {
        return bodyRigidbody != null
            || bodyJoint != null
            || leftArmJoint != null
            || rightArmJoint != null
            || leftLegJoint != null
            || rightLegJoint != null;
    }
}
