using UnityEngine;
using Zenject;

public sealed class UIObjectPool : MonoBehaviourObjectPool<WindowView, FactoryMonoBehaviourBase<WindowView>>
{
    public override string RootFolder => "UI";

    public UIObjectPool(PoolParentContainersManager containersManager, FactoryMonoBehaviourBase<WindowView> factory) : base(containersManager, factory)
    {

    }
}
