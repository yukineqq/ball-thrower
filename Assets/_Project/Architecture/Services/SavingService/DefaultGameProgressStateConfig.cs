using UnityEngine;

[CreateAssetMenu(fileName = "New DefaultGameProgressStateConfig", menuName = "Configs/Gameplay/DefaultGameProgressStateConfig")]
public sealed class DefaultGameProgressStateConfig : ScriptableObject
{
    [field: SerializeField] public int Balance { get; private set; } = 5;
    [field: SerializeField] public int Highscore { get; private set; } = 0;
}
