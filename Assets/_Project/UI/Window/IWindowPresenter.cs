using System;
using UnityEngine;
using Zenject;

public interface IWindowPresenter : IInitializable, IDisposable
{
    public string WindowViewPrefabTitle { get; }
    public void Close();
    public void SetParent(Transform parent);
}
