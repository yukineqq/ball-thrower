using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class GolfFinishState : GolfState
{
    private readonly SceneStateMachine _sceneStateMachine;

    public GolfFinishState(GolfStateMachine stateMachine, GolfReusableData reusableData, SceneStateMachine sceneStateMachine) : base(stateMachine, reusableData)
    {
        _sceneStateMachine = sceneStateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        PreventGolfHoleModeTransitions();

        _reusableData.Ball.SetIsKinematic(true);
        _reusableData.Ball.Teleport(_reusableData.Board.BallSpawnpoint);

        _reusableData.SessionHelper.SaveProgress();
        
        _sceneStateMachine.Enter<GameplayFinishState>().Forget();
    }
}
