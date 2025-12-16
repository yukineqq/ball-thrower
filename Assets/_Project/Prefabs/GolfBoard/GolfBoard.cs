using System.Collections.Generic;
using UnityEngine;

public sealed class GolfBoard : Entity
{
    [SerializeField] private GolfBoardResizer _helper;

    public Vector2 BoardSize => _helper.Size;
    public Vector3 BallSpawnpoint => _helper.BallSpawnpoint;

    public void SetDimensions(float width, float length)
    {
        _helper.SetDimensions(width, length);
    }

    public List<Vector3> GetSpawnPositions()
    {
        return _helper.GetSpawnPositions();
    }
}
