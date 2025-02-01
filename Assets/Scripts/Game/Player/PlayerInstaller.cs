using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerManagerPrefab;

    public override void InstallBindings()
    {
        if (_playerPrefab == null)
        {
            Debug.LogError("PlayerPrefab is not assigned in GameInstaller!");
            return;
        }

        Container.Bind<RoomManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerManager>()
                     .FromComponentInNewPrefab(_playerManagerPrefab)
                     .AsTransient();
        Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle().WithArguments(_playerPrefab);
    }
}
