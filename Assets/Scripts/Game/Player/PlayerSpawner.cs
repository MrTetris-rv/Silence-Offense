using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private Transform playerListContent;

    //public Dictionary
    public static event Action OnPlayerReady;

    //private void Start()
    //{
    //    PhotonNetwork.ConnectUsingSettings();
    //}

    //public override void OnConnectedToMaster()
    //{
    //    PhotonNetwork.JoinOrCreateRoom("FirstRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    //}

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    Debug.Log($"Player_" + Guid.NewGuid());
    //    PhotonNetwork.NickName = $"Player_" + Guid.NewGuid();
    //    GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.transform.position, Quaternion.identity);
    //    OnPlayerReady?.Invoke();

    //    Player[] players = PhotonNetwork.PlayerList;
    //    foreach(var playerItem in players)
    //    {
    //        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(playerItem);
    //    }
    //}

  
    //public override void OnJoinedLobby()
    //{
    //    Debug.Log($"Player_" + Guid.NewGuid());
    //    PhotonNetwork.NickName = $"Player_" + Guid.NewGuid();
    //}

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    //}
}
