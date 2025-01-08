using Photon.Pun;
using System;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TeamSelectionData teamSelectionData; 
    [SerializeField] private Transform redTeamSpawnPoint;
    [SerializeField] private Transform blueTeamSpawnPoint;

    private PhotonView _photonView;
    private Transform _spawnPoint = null;

    public static event Action OnPlayerReady;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        TeamManager.OnTeamChoiceReady += TeamChoiceReady;
    }

    private void OnDisable()
    {
        TeamManager.OnTeamChoiceReady -= TeamChoiceReady;
    }

    private void CreateController()
    {
        //if (teamSelectionData == null || string.IsNullOrEmpty(teamSelectionData.selectedTeam))
        //{
        //    Debug.LogError("The team wasn't been chosen!");
        //    return;
        //}

       if(_spawnPoint != null)
        {
            GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), _spawnPoint.position, _spawnPoint.rotation);

            if (player != null)
            {
                OnPlayerReady?.Invoke();
            }
        }
    }

    private void TeamChoiceReady()
    {
        if (teamSelectionData.selectedTeam == "Terrorist")
        {
            _spawnPoint = redTeamSpawnPoint;
        }
        else if (teamSelectionData.selectedTeam == "Counter-terrorist")
        {
            _spawnPoint = blueTeamSpawnPoint;
        }
        else
        {
            Debug.LogError($"Team {teamSelectionData.selectedTeam} is not defined!");
            return;
        }

        if (_photonView.IsMine)
        {
            CreateController();
        }
    }
}
