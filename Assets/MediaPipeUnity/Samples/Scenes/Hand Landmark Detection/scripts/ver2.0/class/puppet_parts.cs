using System;
using UnityEngine;

[Serializable]
public class puppet_parts
{
    [SerializeField] private Rigidbody2D head;
    [SerializeField] private Rigidbody2D upperBody;
    [SerializeField] private Rigidbody2D lowerBody;
    [SerializeField] private Rigidbody2D leftUpperArm;
    [SerializeField] private Rigidbody2D leftForearm;
    [SerializeField] private Rigidbody2D rightUpperArm;
    [SerializeField] private Rigidbody2D rightForearm;
    [SerializeField] private Rigidbody2D leftThigh;
    [SerializeField] private Rigidbody2D leftShin;
    [SerializeField] private Rigidbody2D rightThigh;
    [SerializeField] private Rigidbody2D rightShin;
    [SerializeField] private Rigidbody2D leftHandEnd;
    [SerializeField] private Rigidbody2D rightHandEnd;
    [SerializeField] private Rigidbody2D leftFootEnd;
    [SerializeField] private Rigidbody2D rightFootEnd;

    public Rigidbody2D headBody => head;
    public Rigidbody2D upperBodyPart => upperBody;
    public Rigidbody2D lowerBodyPart => lowerBody;
    public Rigidbody2D leftUpperArmPart => leftUpperArm;
    public Rigidbody2D leftForearmPart => leftForearm;
    public Rigidbody2D rightUpperArmPart => rightUpperArm;
    public Rigidbody2D rightForearmPart => rightForearm;
    public Rigidbody2D leftThighPart => leftThigh;
    public Rigidbody2D leftShinPart => leftShin;
    public Rigidbody2D rightThighPart => rightThigh;
    public Rigidbody2D rightShinPart => rightShin;
    public Rigidbody2D leftHandEndPart => leftHandEnd;
    public Rigidbody2D rightHandEndPart => rightHandEnd;
    public Rigidbody2D leftFootEndPart => leftFootEnd;
    public Rigidbody2D rightFootEndPart => rightFootEnd;
    public Rigidbody2D bodyCore => upperBody != null ? upperBody : lowerBody;

    public bool hasAnyPart()
    {
        return head != null
            || upperBody != null
            || lowerBody != null
            || leftUpperArm != null
            || leftForearm != null
            || rightUpperArm != null
            || rightForearm != null
            || leftThigh != null
            || leftShin != null
            || rightThigh != null
            || rightShin != null
            || leftHandEnd != null
            || rightHandEnd != null
            || leftFootEnd != null
            || rightFootEnd != null;
    }
}
