using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkingSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pingInfoText;

    private bool _gameSceneIsReady = false;

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerReady += GameSceneIsReady;
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerReady -= GameSceneIsReady;
        _gameSceneIsReady=false;
    }

    private void FixedUpdate()
    {
        if (_gameSceneIsReady)
        {
            pingInfoText.text = "Ping: " + PhotonNetwork.GetPing().ToString();
        }  
    }

    private void GameSceneIsReady()
    {
        _gameSceneIsReady = true;
    }
}
