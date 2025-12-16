using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IExitableState
{
    public UniTask Enter();
    public UniTask Exit();
}
