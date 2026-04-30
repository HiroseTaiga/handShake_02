using System;
using UnityEngine;

[Serializable]
public class finger_target_set
{
    [SerializeField] private Rigidbody2D thumbTarget;
    [SerializeField] private Rigidbody2D indexTarget;
    [SerializeField] private Rigidbody2D middleTarget;
    [SerializeField] private Rigidbody2D ringTarget;
    [SerializeField] private Rigidbody2D littleTarget;

    public Rigidbody2D thumb => thumbTarget;
    public Rigidbody2D index => indexTarget;
    public Rigidbody2D middle => middleTarget;
    public Rigidbody2D ring => ringTarget;
    public Rigidbody2D little => littleTarget;

    public Rigidbody2D left_hand_target => indexTarget;
    public Rigidbody2D right_hand_target => ringTarget;
    public Rigidbody2D left_foot_target => thumbTarget;
    public Rigidbody2D right_foot_target => littleTarget;
    public Rigidbody2D head_target => middleTarget;

    public bool hasAnyTarget()
    {
        return thumbTarget != null
            || indexTarget != null
            || middleTarget != null
            || ringTarget != null
            || littleTarget != null;
    }
}
