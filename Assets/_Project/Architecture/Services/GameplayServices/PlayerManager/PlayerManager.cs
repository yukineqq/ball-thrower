using UnityEngine;
using Zenject;

public sealed class PlayerManager
{
    private readonly ISpawnService _spawnService;
    private Player _player;

    public PlayerManager(ISpawnService spawnService)
    {
        _spawnService = spawnService;
    }

    public void EnsurePlayerCreation()
    {
        if (_player == null)
        {
            _player = _spawnService.Spawn<Player>(typeof(Player).Name);
        }
    }

    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        if (_player != null)
        {
            _player.gameObject.SetActive(true);
            _player.transform.SetPositionAndRotation(position, rotation);
        }
    }

    public void DespawnPlayer()
    {
        if (_player != null)
        {
            _player.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _player.gameObject.SetActive(false);
        }
    }
}
