using System;
using System.Threading;
using UnityEngine;

public sealed class CancellationService : IDisposable
{
    private CancellationTokenSource _applicationExitCancellationTokenSource;
    private CancellationTokenSource _currentGlobalContextCancellationTokenSource;

    public CancellationToken ApplicationExitCancellationToken => _applicationExitCancellationTokenSource.Token;
    public CancellationToken CurrentGlobalContextCancellationToken => _currentGlobalContextCancellationTokenSource.Token;

    public CancellationService()
    {
        _applicationExitCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);

        UpdateCurrentContextCancellationToken();
    }

    public void Dispose()
    {
        CancelCurrentContextCancellationToken();
    }

    public void EmergencyApplicationExitCancellationTokenCancel()
    {
        _applicationExitCancellationTokenSource.Cancel();
    }

    public void UpdateCurrentContextCancellationToken()
    {
        if (_currentGlobalContextCancellationTokenSource != null)
        {
            _currentGlobalContextCancellationTokenSource.Cancel();
            _currentGlobalContextCancellationTokenSource.Dispose();
        }

        _currentGlobalContextCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_applicationExitCancellationTokenSource.Token);
    }

    public void CancelCurrentContextCancellationToken()
    {
        if (_currentGlobalContextCancellationTokenSource == null)
        {
            return;
        }

        _currentGlobalContextCancellationTokenSource.Cancel();
    }
}
