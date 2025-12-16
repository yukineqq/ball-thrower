using System.Collections.Generic;
using UnityEngine;

public sealed class TrajectoryLine : Entity
{
    [SerializeField] private LineRenderer _line;
    public LineRenderer LineRenderer => _line;

    private void Awake()
    {
        _line.startWidth = 0.5f;
        _line.endWidth = 0.5f;
    }

    public void ShootLaser(Vector3 position, Vector3 direction)
    {
        
    }
}
