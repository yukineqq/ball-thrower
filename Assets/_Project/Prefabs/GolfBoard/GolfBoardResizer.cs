using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class GolfBoardResizer : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private BoxCollider _frontCollider;
    [SerializeField] private BoxCollider _backCollider;
    [SerializeField] private BoxCollider _leftCollider;
    [SerializeField] private BoxCollider _rightCollider;
    [SerializeField] private BoxCollider _topCollider;
    [SerializeField] private Transform _ballSpawnpoint;

    private GolfBoardConfig _config;

    public Vector3 BallSpawnpoint => _ballSpawnpoint.transform.position;
    public Vector2 Size { get; private set; }

    [Inject]
    public void Construct(IStaticDataService staticDataService)
    {
        _config = staticDataService.GetResourcesSingleConfigByPath<GolfBoardConfig>("Configs/Gameplay/GolfBoardConfig");
    }

    public void SetDimensions(float width, float length)
    {
        width = Mathf.Clamp(width, _config.BoardMinWidth, _config.BoardMaxWidth);
        length = Mathf.Clamp(length, _config.BoardMinLength, _config.BoardMaxLength);

        Size = new Vector2(width, length);

        _ballSpawnpoint.transform.SetLocalPositionAndRotation(Vector3.forward * _config.BallSpawnPointOffset, Quaternion.identity);

        float halfWidth = width * 0.5f;
        float halfLength = length * 0.5f;

        Vector3 frontAndBackSize = new Vector3(width, _config.CollidersHeight, 1f);
        Vector3 leftAndRightSize = new Vector3(1f, _config.CollidersHeight, length);

        _ground.transform.localPosition = Vector3.forward * halfLength;
        _ground.transform.localScale = new Vector3(width, 1f, length);

        _topCollider.transform.localPosition = Vector3.forward * halfLength + Vector3.up * _config.CollidersHeight * 0.5f;
        _topCollider.size = new Vector3(width, 1f, length);

        _frontCollider.transform.localPosition = Vector3.forward * length;
        _frontCollider.size = frontAndBackSize;

        _backCollider.transform.localPosition = Vector3.zero;
        _backCollider.size = frontAndBackSize;

        _leftCollider.transform.localPosition = Vector3.left * halfWidth + Vector3.forward * halfLength;
        _leftCollider.size = leftAndRightSize;

        _rightCollider.transform.localPosition = Vector3.right * halfWidth + Vector3.forward * halfLength;
        _rightCollider.size = leftAndRightSize;
    }

    public List<Vector3> GetSpawnPositions()
    {
        List<Vector3> spawnPositions = new List<Vector3>();

        Vector3 topLeftCorner = transform.position + Vector3.forward * Size.y + Vector3.left * (Size.x * 0.5f);
        Vector3 bottomRightCorner = transform.position + Vector3.right * (Size.x * 0.5f);

        int minX = Mathf.RoundToInt(Mathf.Min(topLeftCorner.x, bottomRightCorner.x));
        int maxX = Mathf.RoundToInt(Mathf.Max(topLeftCorner.x, bottomRightCorner.x));
        minX++;
        maxX--;

        int minZ = Mathf.RoundToInt(Mathf.Min(bottomRightCorner.z, topLeftCorner.z));
        int maxZ = Mathf.RoundToInt(Mathf.Max(bottomRightCorner.z, topLeftCorner.z));
        minZ += Mathf.RoundToInt(_ballSpawnpoint.position.z * 2f);
        maxZ--;

        for (int x = minX; x < maxX; x++)
        {
            for (int z = minZ; z < maxZ; z++)
            {
                spawnPositions.Add(new Vector3(x, 0.5f, z));
            }
        }

        return spawnPositions;
    }
}
