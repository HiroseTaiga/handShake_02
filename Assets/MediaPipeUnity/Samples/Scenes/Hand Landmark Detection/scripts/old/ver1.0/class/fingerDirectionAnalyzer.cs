using UnityEngine;

public class fingerDirectionAnalyzer
{
    private readonly directionConfig config;

    public fingerDirectionAnalyzer(directionConfig config)
    {
        this.config = config;
    }

    public fingerDirection analyze(Vector3 tipPosition, Vector3 basePosition, handType hand)
    {
        Vector3 fingerVector = tipPosition - basePosition;

        Vector2 v = new Vector2(fingerVector.x, fingerVector.y);

        float magnitude = v.magnitude;

        if (magnitude < config.minMagnitude)
        {
            return fingerDirection.center;
        }

        v.Normalize();

        float x = v.x;
        float y = v.y;

        if (Mathf.Abs(x) < config.axisTolerance)
        {
            x = 0f;
        }

        if (Mathf.Abs(y) < config.axisTolerance)
        {
            y = 0f;
        }

        if (hand == handType.left)
        {
            x = -x;
        }

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0f)
            {
                return fingerDirection.right;
            }
            else if (x < 0f)
            {
                return fingerDirection.left;
            }
        }
        else
        {
            if (y > 0f)
            {
                return fingerDirection.up;
            }
            else if (y < 0f)
            {
                return fingerDirection.down;
            }
        }

        return fingerDirection.center;
    }
}