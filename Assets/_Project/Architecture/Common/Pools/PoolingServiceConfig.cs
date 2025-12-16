using UnityEngine;

[CreateAssetMenu(fileName = "New PoolingServiceConfig", menuName = "Configs/Infrastructure/PoolingServiceConfig")]
public sealed class PoolingServiceConfig : ScriptableObject
{
    [field: SerializeField] public int InitialPoolCapacity { get; private set; } = 20;
}
