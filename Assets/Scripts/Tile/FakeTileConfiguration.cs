using UnityEngine;

public class FakeTileConfiguration
{
    public Vector2 StartPosition { get; private set; }
    public Vector2 TargetPosition { get; private set; }
    public int Value { get; private set; }
    public Vector3 Scale { get; private set; }

    public FakeTileConfiguration(int value, Vector2 startPosition, Vector2 targetPosition, Vector3 scale)
    {
        StartPosition = startPosition;
        TargetPosition = targetPosition;
        Value = value;
        Scale = scale;
    }
}

