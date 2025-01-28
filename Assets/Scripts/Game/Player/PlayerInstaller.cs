using System.IO;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
   private const string PlayerPrefabPath = "PhotonPrefabs/Player"; // Укажите путь к вашему префабу

    public override void InstallBindings()
    {
        Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle().WithArguments(PlayerPrefabPath);
    }
}
