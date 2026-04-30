using UnityEngine;

public class fingerState
{
    public Vector3 tipPosition { get; }
    public Vector3 basePosition { get; }
    public fingerDirection direction { get; private set; }

    public fingerState(Vector3 tipPosition, Vector3 basePosition)
    {
        this.tipPosition = tipPosition;
        this.basePosition = basePosition;
        direction = fingerDirection.center;
    }

    public void setDirection(fingerDirection dir)
    {
        direction = dir;
    }
}