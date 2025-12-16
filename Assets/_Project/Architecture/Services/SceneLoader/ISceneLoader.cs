using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    public string CurrentSceneName { get; }
    public UniTask Load(string targetScene);
    public UniTask<Scene> LoadAdditiveFromBundle(string additiveScene, bool activationOnLoad = true);
    public UniTask UnloadAdditiveFromBundle(string additiveScene);
    public UniTask ToLoadingScene(CancellationToken cancellationToken);
}
