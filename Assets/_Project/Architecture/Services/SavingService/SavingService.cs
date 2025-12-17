using System.IO;
using UnityEngine;
using Zenject;

public sealed class SavingService : ISavingService
{
    private const string SAVE_KEY = "save.ballthrower";
    private DefaultGameProgressStateConfig _config;
    private GameProgressState _gameProgressState;
    public GameProgressStateProxy GameProgressStateProxy { get; private set; }

    public SavingService(IStaticDataService staticDataService)
    {
        _config = staticDataService.GetResourcesSingleConfigByPath<DefaultGameProgressStateConfig>("Configs/Gameplay/DefaultGameProgressStateConfig"); ;
    }

    public GameProgressStateProxy LoadProgress()
    {
        if (!File.Exists(GetSavesPath(SAVE_KEY)))
        {
            GameProgressStateProxy = CreateDefaultProgressState();
            Debug.Log("created default game state");

            SaveProgress();
        }

        var json = File.ReadAllText(GetSavesPath(SAVE_KEY));
        _gameProgressState = JsonUtility.FromJson<GameProgressState>(json);
        GameProgressStateProxy = new GameProgressStateProxy(_gameProgressState);
        Debug.Log("game state loaded");

        return GameProgressStateProxy;
    }

    public void SaveProgress()
    {
        var json = JsonUtility.ToJson(_gameProgressState, true);

        File.WriteAllText(GetSavesPath(SAVE_KEY), json);
    }

    private GameProgressStateProxy CreateDefaultProgressState()
    {
        if (_config == null) { Debug.Log("LSKDJLFKJDLFKJDLKFJLKDJLKFJLKKJFDLDKLKJF"); }
        _gameProgressState = new GameProgressState() { Balance = _config.Balance, HighScore = _config.Highscore };

        return new GameProgressStateProxy(_gameProgressState);
    }

    public void ResetGame()
    {
        GameProgressStateProxy = CreateDefaultProgressState();

        SaveProgress();
    }

    private void DeleteAllSaves()
    {
        Directory.Delete(GetSavesPath(), true);
    }

    private string GetSavesPath(string saveName = "")
    {
        string savesPath = GetRootPath("Saves");

        if (!string.IsNullOrEmpty(saveName))
        {
            savesPath = Path.Combine(savesPath, saveName);
        }

        return savesPath;
    }

    private string GetRootPath(string directoryWithinRootTitle = "")
    {
        if (string.IsNullOrEmpty(directoryWithinRootTitle))
        {
            return Directory.GetParent(Application.dataPath).FullName;
        }

        string directoryWithinRootFolderPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, directoryWithinRootTitle);

        if (!Directory.Exists(directoryWithinRootFolderPath))
        {
            Directory.CreateDirectory(directoryWithinRootFolderPath);
        }

        return directoryWithinRootFolderPath;
    }

}
