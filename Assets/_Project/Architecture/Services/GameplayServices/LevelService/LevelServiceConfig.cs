using UnityEngine;

[CreateAssetMenu(fileName = "New LevelConfig", menuName = "Configs/Infrastructure/LevelServiceConfig")]
public sealed class LevelServiceConfig : ScriptableObject
{
    [field: SerializeField] public Vector2Int BoardSize { get; private set; } = new Vector2Int(10, 24);
    [field: SerializeField] public int GolfHolesCount { get; private set; } = 6;
    [field: SerializeField] public float Timer { get; private set; } = 30f;
    [field: SerializeField] public Vector3 CameraPosition { get; private set; } = new Vector3(0f, 10f, -12f);
    [field: SerializeField] public Vector3 CameraRotation { get; private set; } = Vector3.right * -20f;
}
