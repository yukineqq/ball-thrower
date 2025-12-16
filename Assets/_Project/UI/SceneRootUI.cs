using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class SceneRootUI : MonoBehaviour, IPopupCloser
{
    [SerializeField] private Transform _screensContainer;
    [SerializeField] private Transform _popupsContainer;

    private readonly List<IWindowPresenter> _windowPresenters = new List<IWindowPresenter>();
    private readonly Stack<IWindowPresenter> _popupPresenters = new Stack<IWindowPresenter>();

    public IReadOnlyList<IWindowPresenter> WindowPresenters => _windowPresenters;
    public IReadOnlyCollection<IWindowPresenter> PopupPresenters => _popupPresenters;

    public event Action<int> PopupsCountChanged;

    private void OnDestroy()
    {
        Cleanup();
    }

    public void RequestClosePopup(IWindowPresenter popup)
    {
        if (_popupPresenters.Count > 0 && _popupPresenters.Peek() == popup)
        {
            ClosePeekPopup();
        }
    }

    public void OpenPopup(IWindowPresenter popup)
    {
        if (_popupPresenters.Contains(popup))
        {
            PopupsCountChanged?.Invoke(_popupPresenters.Count);
            return;
        }

        _popupPresenters.Push(popup);
        popup.SetParent(_popupsContainer);
        PopupsCountChanged?.Invoke(_popupPresenters.Count);
    }

    public void ClosePeekPopup()
    {
        if (_popupPresenters.Count <= 0)
        {
            PopupsCountChanged?.Invoke(_popupPresenters.Count);
            return;
        }

        Debug.Log("peek popup closed");
        _popupPresenters.Pop().Close();
        PopupsCountChanged?.Invoke(_popupPresenters.Count);
    }

    public void CloseAllPopups()
    {
        while (_popupPresenters.Count > 0)
        {
            _popupPresenters.Pop().Close();
        }
    }

    public void CloseAllPopupsAndNotify()
    {
        CloseAllPopups();
        PopupsCountChanged?.Invoke(_popupPresenters.Count);
    }

    public void AttachScreen(IWindowPresenter screen)
    {
        if (_windowPresenters.Contains(screen))
        {
            return;
        }

        _windowPresenters.Add(screen);
        screen.SetParent(_screensContainer);
    }

    public void DetachScreen(IWindowPresenter screen)
    {
        if (_windowPresenters.Contains(screen))
        {
            _windowPresenters.Remove(screen);
            screen.Close();
        }
    }

    public void CloseAllScreens()
    {
        for (int i = 0; i < _windowPresenters.Count; i++)
        {
            _windowPresenters[i].Close();
        }

        _windowPresenters.Clear();
    }

    public void Cleanup()
    {
        CloseAllPopups();
        CloseAllScreens();
    }
}
