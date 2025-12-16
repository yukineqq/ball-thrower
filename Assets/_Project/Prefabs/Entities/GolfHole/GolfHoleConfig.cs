using UnityEngine;

[CreateAssetMenu(fileName = "New GolfHoleConfig", menuName = "Configs/Gameplay/GolfHoleConfig")]
public sealed class GolfHoleConfig : ScriptableObject
{
    [field: SerializeField] public float ModeTransitionRandomizationInterval { get; private set; } = 1f;
    [field: SerializeField, Range(0f, 100f)] public float TransitionChance { get; private set; } = 50f;
    [field: SerializeField] public Color GolfHolePositiveColor { get; private set; } = Color.blue;
    [field: SerializeField] public Color GolfHoleNegativeColor { get; private set; } = Color.red;
    [field: SerializeField] public Color IndicatorEndColor { get; private set; } = Color.white;
    [field: SerializeField] public Color IndicatorRegularColor { get; private set; } = Color.black;
}
