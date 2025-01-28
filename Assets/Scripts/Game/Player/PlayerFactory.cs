using Photon.Pun;
using UnityEngine;

public class PlayerFactory : IPlayerFactory
{
    private readonly GameObject _playerPrefab;

    public PlayerFactory(string playerPrefabPath)
    {
        _playerPrefab = Resources.Load<GameObject>(playerPrefabPath);
        if (_playerPrefab == null)
        {
            Debug.LogError($"Player prefab not found at path: {playerPrefabPath}");
        }
    }

    public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
    {
        return PhotonNetwork.Instantiate(_playerPrefab.name, position, rotation, 0);
    }
}