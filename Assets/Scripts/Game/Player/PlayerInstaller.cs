using System.IO;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
   private const string PlayerPrefabPath = "PhotonPrefabs/Player"; // ������� ���� � ������ �������

    public override void InstallBindings()
    {
        Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle().WithArguments(PlayerPrefabPath);
    }
}
