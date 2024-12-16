using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject spawnPoint;

    public static event Action<MinimapController> OnPlayerReady;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("FirstRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.transform.position, Quaternion.identity);

        MinimapController minimapController = player.GetComponent<MinimapController>();

        if (minimapController != null)
        {
            OnPlayerReady?.Invoke(minimapController);
        }
    }
}
