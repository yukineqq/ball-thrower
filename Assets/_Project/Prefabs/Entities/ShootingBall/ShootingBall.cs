using System;
using System.Collections;
using UnityEngine;

public sealed class ShootingBall : Entity
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SphereCollider _collider;

    private Coroutine _slowdownCoroutine;

    public event Action<GolfHole> HoleHit;
    public event Action Timeout;

    public void Shoot(Vector3 direction, float force, float timeoutDelay)
    {
        SetIsKinematic(false);

        _rigidbody.AddForce(direction * force, ForceMode.Impulse);

        _slowdownCoroutine = StartCoroutine(OnTimeout(timeoutDelay));
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position + _collider.radius * 2f * Vector3.up;
        transform.rotation = Quaternion.identity;
    }

    public void SetIsKinematic(bool value)
    {
        _rigidbody.isKinematic = value;
        _rigidbody.useGravity = !value;
        _rigidbody.interpolation = value ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<GolfHole>(out GolfHole hole))
        {
            if (_slowdownCoroutine != null)
            {
                StopCoroutine(_slowdownCoroutine);
            }

            HoleHit?.Invoke(hole);
        }
    }

    private IEnumerator OnTimeout(float timeoutDelay = 5f)
    {
        yield return new WaitForSeconds(timeoutDelay);

        Timeout?.Invoke();
    }
}
