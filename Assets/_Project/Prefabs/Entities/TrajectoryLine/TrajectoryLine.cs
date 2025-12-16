using System.Collections.Generic;
using UnityEngine;

public sealed class TrajectoryLine : Entity
{
    [SerializeField] private LineRenderer _line;
    public LineRenderer LineRenderer => _line;

    private void Awake()
    {
        _line.startWidth = 0.5f;
        _line.endWidth = 0.5f;
    }

    public void ShootLaser(Vector3 touchStartPosition, Vector3 touchPosition, Transform ballTransform)
    {
        Vector2 differenceVector = touchStartPosition - touchPosition;
        Vector3 direction = new Vector3(differenceVector.x, 0f, differenceVector.y).normalized;

        Vector3 positionAtTime = ballTransform.position;
        transform.position = positionAtTime;

        _line.positionCount = 20;

        float power = 4f;
        Vector3 velocity = direction * power;

        int reflections = 0;

        for (int i = 0; i < _line.positionCount; i++)
        {
            Vector3 newPosition = positionAtTime + velocity * 0.1f;

            RaycastHit hit;
            if (Physics.Raycast(positionAtTime, velocity, out hit, Vector3.Distance(positionAtTime, newPosition)))
            {
                _line.SetPosition(i, hit.point);
                positionAtTime = hit.point;

                velocity = Vector3.Reflect(velocity, hit.normal);

                reflections++;

                if (reflections >= 2)
                {
                    break;
                }
            }
            else
            {
                _line.SetPosition(i, newPosition);
                positionAtTime = newPosition;
            }
        }

    }
}
