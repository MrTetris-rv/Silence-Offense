using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject spawnPoint;
    //[SerializeField] private GameObject playerListItemPrefab;
    //[SerializeField] private Transform playerListContent;

    public static event Action OnPlayerReady;

    private void Awake()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.transform.position, Quaternion.identity);

        if (player != null)
        {
            OnPlayerReady?.Invoke();
        }
      
    }
}
