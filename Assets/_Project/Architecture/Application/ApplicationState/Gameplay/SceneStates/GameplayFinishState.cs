using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayFinishState : BaseSceneState
{
    private readonly GameplayUIManager _uiManager;
    public GameplayFinishState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
        UIRootView uiRoot, GameplayUIManager uiManager) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _uiManager = uiManager;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _uiManager.CreatePopup<GameplayFinishPopupPresenter>();
    }
}
