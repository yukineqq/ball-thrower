using UnityEngine;

public interface ISavingService
{
    public GameProgressStateProxy GameProgressStateProxy { get; }
    public GameProgressStateProxy LoadProgress();
    public void SaveProgress();
    public void ResetGame();
}
