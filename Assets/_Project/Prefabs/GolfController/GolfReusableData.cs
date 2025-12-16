using System.Collections.Generic;
using UnityEngine;

public sealed class GolfReusableData
{
    public GolfBoard Board { get; }
    public List<GolfHole> GolfHoles { get; }
    public ShootingBall Ball { get; }
    public GolfTimer Timer { get; }
    public PlayingSessionHelper SessionHelper { get; }
    public float GolfHolesAmount { get; set; }
    public float BallTimeoutDelay { get; set; }
    public float ShootingForce { get; set; }
    public Vector2 CurrentTouchStartPosition { get; set; }
    public Vector2 CurrentTouchReleasePosition { get; set; }

    public GolfReusableData(GolfBoard board, ShootingBall ball, GolfTimer timer, PlayingSessionHelper sessionHelper)
    {
        Board = board;
        GolfHoles = new List<GolfHole>();
        Ball = ball;
        Timer = timer;
        SessionHelper = sessionHelper;
    }
}
