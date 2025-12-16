using UnityEngine;
using UnityEngine.UI;

public abstract class PopupView : WindowView
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _alternativeCloseButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(RequestClose);
        }

        if (_alternativeCloseButton != null)
        {
            _alternativeCloseButton.onClick.AddListener(RequestClose);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveListener(RequestClose);
        }

        if (_alternativeCloseButton != null)
        {
            _alternativeCloseButton.onClick.RemoveListener(RequestClose);
        }
    }
}
