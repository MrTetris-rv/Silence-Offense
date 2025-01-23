using UnityEngine;
using Zenject;

public class GaneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MainMenuManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RoomManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<NetworkManager>().FromComponentInHierarchy().AsSingle();
    }
}