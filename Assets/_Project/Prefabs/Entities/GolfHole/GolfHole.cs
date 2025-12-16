using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using Zenject;

public sealed class GolfHole : Entity
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private MeshRenderer _indicatorRenderer;
    private CancellationTokenSource _cts;
    private GolfHoleConfig _config;
    private float _counter = 0;
    private bool _busy = false;
    private bool _transitionAllowed = false;
    private Sequence _modeTransitionTween;

    public bool IsNegative { get; private set; } = false;

    [Inject]
    public void Construct(IStaticDataService staticDataService)
    {
        _config = staticDataService.GetResourcesSingleConfigByPath<GolfHoleConfig>("Configs/Gameplay/GolfHoleConfig");
    }

    private void Awake()
    {
        ResetCts();
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    private void Update()
    {
        HanldeModeTransitionRandomization();
    }

    public void AllowModeTransition()
    {
        _transitionAllowed = true;
    }

    public void PreventModeTransition()
    {
        _transitionAllowed = false;

        ResetCts();
    }

    public async UniTask ChangeMode(bool isNegative, bool immediately, float transitionDuration = 1f)
    {
        if (_busy)
        {
            return;
        }

        Color initialGolfHoleColor = _renderer.material.color;

        Color golfHoleEndColor = isNegative ? _config.GolfHoleNegativeColor : _config.GolfHolePositiveColor;
        Color indicatorEndColor = _config.IndicatorEndColor;

        if (immediately)
        {
            IsNegative = isNegative;

            _renderer.material.color = golfHoleEndColor;
            _indicatorRenderer.material.color = _config.IndicatorRegularColor;
            return;
        }

        try
        {
            _busy = true;

            _modeTransitionTween = DOTween.Sequence()
                .Join(_indicatorRenderer.material.DOColor(indicatorEndColor, transitionDuration));

            _modeTransitionTween.Play();

            await _modeTransitionTween.AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_cts.Token);

            IsNegative = isNegative;
        }
        finally
        {
            _busy = false;

            if (_modeTransitionTween.IsComplete())
            {
                _renderer.material.color = golfHoleEndColor;
            }
            else
            {
                _renderer.material.color = initialGolfHoleColor;
            }

            _indicatorRenderer.material.color = _config.IndicatorRegularColor;

            FinishTween(_modeTransitionTween);
        }
    }

    private void HanldeModeTransitionRandomization()
    {
        if (!_transitionAllowed)
        {
            return;
        }

        if (!_busy)
        {
            _counter += Time.deltaTime;
        }

        if (_counter >= _config.ModeTransitionRandomizationInterval)
        {
            _counter = 0f;
            if (Random.Range(0f, 100f) <= _config.TransitionChance)
            {
                ChangeMode(!IsNegative, false, _config.ModeTransitionRandomizationInterval).Forget();
            }
        }
    }

    private void ResetCts()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        _cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
    }

    private void FinishTween(Tween tween)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }
    }
}
