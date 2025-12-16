using UnityEngine;
using Zenject;

public sealed class GameplayUIManager : UIManager
{
    public GameplayUIManager(IInstantiator instantiator, SceneRootUI sceneRootUI, UIPresenterFactory uiFactory) : base(instantiator, sceneRootUI, uiFactory)
    {

    }
}
