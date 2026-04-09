using System;
using UnityEngine;

[Serializable]
public class ragdollWalkMotion
{
    [SerializeField] private float legSwingAngle = 25f;
    [SerializeField] private float legSwingSpeed = 6f;
    [SerializeField] private float armSwingAngle = 12f;
    [SerializeField] private float armSwingSpeed = 6f;
    [SerializeField] private float motorSpeedGain = 8f;
    [SerializeField] private float maxMotorSpeed = 200f;
    [SerializeField] private float armMaxMotorTorque = 200f;
    [SerializeField] private float legMaxMotorTorque = 300f;
    [SerializeField] private float idleReturnTolerance = 1f;
    [SerializeField] private float idleBodyAngle;
    [SerializeField] private float idleLeftArmAngle;
    [SerializeField] private float idleRightArmAngle;
    [SerializeField] private float idleLeftLegAngle;
    [SerializeField] private float idleRightLegAngle;
    [SerializeField] private float walkBodyBaseAngle;
    [SerializeField] private float walkLeftArmBaseAngle;
    [SerializeField] private float walkRightArmBaseAngle;
    [SerializeField] private float walkLeftLegBaseAngle;
    [SerializeField] private float walkRightLegBaseAngle;

    [NonSerialized] private float motionTime;

    public void updateMotion(ragdollWalkParts parts, bool isWalking, int moveDirection, float deltaTime)
    {
        if (parts == null || !parts.hasAnyPart())
        {
            return;
        }

        if (isWalking)
        {
            motionTime += deltaTime;
            updateWalkingMotors(parts, moveDirection);
            return;
        }

        motionTime = 0f;
        updateIdleMotors(parts);
    }

    private void updateWalkingMotors(ragdollWalkParts parts, int moveDirection)
    {
        float legWave = Mathf.Sin(motionTime * legSwingSpeed);
        float armWave = Mathf.Sin(motionTime * armSwingSpeed);
        int direction = sanitizeDirection(moveDirection);

        applyBodyMotor(parts.bodyHinge, walkBodyBaseAngle * direction, legMaxMotorTorque);
        applyLimbMotor(parts.leftLeg, walkLeftLegBaseAngle + legWave * legSwingAngle, legMaxMotorTorque);
        applyLimbMotor(parts.rightLeg, walkRightLegBaseAngle - legWave * legSwingAngle, legMaxMotorTorque);
        applyLimbMotor(parts.leftArm, walkLeftArmBaseAngle - armWave * armSwingAngle, armMaxMotorTorque);
        applyLimbMotor(parts.rightArm, walkRightArmBaseAngle + armWave * armSwingAngle, armMaxMotorTorque);
    }

    private void updateIdleMotors(ragdollWalkParts parts)
    {
        applyBodyMotor(parts.bodyHinge, idleBodyAngle, legMaxMotorTorque);
        applyLimbMotor(parts.leftArm, idleLeftArmAngle, armMaxMotorTorque);
        applyLimbMotor(parts.rightArm, idleRightArmAngle, armMaxMotorTorque);
        applyLimbMotor(parts.leftLeg, idleLeftLegAngle, legMaxMotorTorque);
        applyLimbMotor(parts.rightLeg, idleRightLegAngle, legMaxMotorTorque);
    }

    private void applyBodyMotor(HingeJoint2D joint, float targetAngle, float maxTorque)
    {
        applyMotor(joint, targetAngle, maxTorque);
    }

    private void applyLimbMotor(HingeJoint2D joint, float targetAngle, float maxTorque)
    {
        applyMotor(joint, targetAngle, maxTorque);
    }

    private void applyMotor(HingeJoint2D joint, float targetAngle, float maxTorque)
    {
        if (joint == null)
        {
            return;
        }

        JointMotor2D motor = joint.motor;
        float currentAngle = joint.jointAngle;
        float angleError = Mathf.DeltaAngle(currentAngle, targetAngle);

        if (Mathf.Abs(angleError) <= idleReturnTolerance)
        {
            motor.motorSpeed = 0f;
        }
        else
        {
            float motorSpeed = angleError * motorSpeedGain;
            motor.motorSpeed = Mathf.Clamp(motorSpeed, -maxMotorSpeed, maxMotorSpeed);
        }

        motor.maxMotorTorque = maxTorque;
        joint.motor = motor;
        joint.useMotor = true;
    }

    private int sanitizeDirection(int direction)
    {
        return direction < 0 ? -1 : 1;
    }
}
