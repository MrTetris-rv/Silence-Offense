using UnityEngine;
using Zenject;

public class PlayerFactory : IPlayerFactory
{
    private readonly DiContainer _container;
    private readonly GameObject _playerPrefab;

    public PlayerFactory(DiContainer container, GameObject playerPrefab)
    {
        _container = container;
        _playerPrefab = playerPrefab;
    }

    public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
    {
        GameObject player = _container.InstantiatePrefab(_playerPrefab, position, rotation, null);
        return player;
    }
}