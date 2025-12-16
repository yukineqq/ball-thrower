using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IStaticDataService
{
    public UniTask InitializeAsync();
    public UniTask<TConfig> LoadSingleConfigByPath<TConfig>(string path) where TConfig : ScriptableObject;
    public TConfig GetResourcesSingleConfigByPath<TConfig>(string configAddress) where TConfig : ScriptableObject;
}
