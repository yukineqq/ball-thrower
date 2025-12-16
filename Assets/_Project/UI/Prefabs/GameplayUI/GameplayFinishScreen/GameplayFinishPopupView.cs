using TMPro;
using UnityEngine;

public sealed class GameplayFinishPopupView : PopupView
{
    [SerializeField] private TextMeshProUGUI _finishScoreText;

    public string FinishText { get => _finishScoreText.text; set => _finishScoreText.text = value; }
}
