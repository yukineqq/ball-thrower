using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GameplayFinishState : BaseSceneState
{
    private readonly GameplayUIManager _uiManager;
    private readonly PlayingSessionHelper _sessionHelper;
    public GameplayFinishState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
        UIRootView uiRoot, GameplayUIManager uiManager, PlayingSessionHelper sessionHelper) : base(gameStateMachine, sceneStateMachine, uiRoot)
    {
        _uiManager = uiManager;
        _sessionHelper = sessionHelper;
    }

    public override async UniTask Enter()
    {
        await base.Enter();

        _sessionHelper.SaveProgress();

        _uiManager.CreatePopup<GameplayFinishPopupPresenter>();
    }
}
