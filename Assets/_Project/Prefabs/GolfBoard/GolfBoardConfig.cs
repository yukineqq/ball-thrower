using UnityEngine;

[CreateAssetMenu(fileName = "New GolfBoardConfig", menuName = "Gameplay/GolfBoardConfig")]
public sealed class GolfBoardConfig : ScriptableObject
{
    [field: SerializeField, Range(4f, 100f)] public float BoardMinWidth { get; private set; } = 4f;
    [field: SerializeField, Range(4f, 100f)] public float BoardMaxWidth { get; private set; } = 10f;
    [field: SerializeField, Range(4f, 100f)] public float BoardMinLength { get; private set; } = 6f;
    [field: SerializeField, Range(4f, 100f)] public float BoardMaxLength { get; private set; } = 30f;
    [field: SerializeField, Range(10f, 50f)] public float CollidersHeight { get; private set; } = 20f;
    [field: SerializeField, Range(1f, 3f)] public float BallSpawnPointOffset { get; private set; } = 2f;
}
