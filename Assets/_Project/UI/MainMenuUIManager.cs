using UnityEngine;
using Zenject;

public sealed class MainMenuUIManager : UIManager
{
    public MainMenuUIManager(IInstantiator instantiator, SceneRootUI sceneRootUI, UIPresenterFactory uiFactory) : base(instantiator, sceneRootUI, uiFactory)
    {

    }
}
