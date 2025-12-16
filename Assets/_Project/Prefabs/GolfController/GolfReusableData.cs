using System.Collections.Generic;
using UnityEngine;

public sealed class GolfReusableData
{
    public GolfBoard Board { get; set; }
    public List<GolfHole> GolfHoles { get; set; }
    public ShootingBall Ball { get; set; }
    public GolfTimer Timer { get; set; }
    public Vector2 CurrentTouchStartPosition { get; set; }
    public Vector2 CurrentTouchReleasePosition { get; set; }
}
