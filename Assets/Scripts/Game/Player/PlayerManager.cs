using Photon.Pun;
using System;
using System.IO;
using UnityEngine;
using Zenject;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TeamSelectionData teamSelectionData;
    [SerializeField] private Transform redTeamSpawnPoint;
    [SerializeField] private Transform blueTeamSpawnPoint;
    [SerializeField] private Transform spawnPoint;

    [Inject]
    private IPlayerFactory _playerFactory;

    private PhotonView _photonView;
    //private Transform _spawnPoint;

    public static event Action OnPlayerReady;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        //_spawnPoint.position = new Vector3(4.57f, 0, 3.22f);
    }

    private void OnEnable()
    {
        //TeamManager.OnTeamChoiceReady += TeamChoiceReady;
        if (_photonView.IsMine) CreateController();
    }

    private void OnDisable()
    {
        //TeamManager.OnTeamChoiceReady -= TeamChoiceReady;
    }

    private void CreateController()
    {
        //if (teamSelectionData == null || string.IsNullOrEmpty(teamSelectionData.selectedTeam))
        //{
        //    Debug.LogError("The team wasn't been chosen!");
        //    return;
        //}

        //if(_spawnPoint != null)
        // {
        _playerFactory.CreatePlayer(spawnPoint.position, Quaternion.identity);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint.position, Quaternion.identity);

        OnPlayerReady?.Invoke();

        // }
    }

    //private void TeamChoiceReady()
    //{
    //    if (string.IsNullOrEmpty(teamSelectionData.selectedTeam))
    //    {
    //        Debug.LogError("Selected team is invalid!");
    //        return;
    //    }

    //    if (_photonView.IsMine)
    //    {
    //switch (teamSelectionData.selectedTeam)
    //{
    //    case "Terrorist":
    //        spawnPoint = redTeamSpawnPoint;
    //        break;
    //    case "Counter-terrorist":
    //        spawnPoint = blueTeamSpawnPoint;
    //        break;
    //    default:
    //        Debug.LogError($"Undefined team: {team}");
    //        break;
    //}
//        if (teamSelectionData.selectedTeam == "Terrorist")
//        {
//            _spawnPoint = redTeamSpawnPoint;
//        }
//        else if (teamSelectionData.selectedTeam == "Counter-terrorist")
//        {
//            _spawnPoint = blueTeamSpawnPoint;
//        }
//        else
//        {
//            Debug.LogError($"Team {teamSelectionData.selectedTeam} is not defined!");
//            return;
//        }

//        string[] teamData = new string[] { PhotonNetwork.LocalPlayer.NickName, teamSelectionData.selectedTeam };
//        _photonView.RPC("SyncTeam", RpcTarget.AllBuffered, teamData);


//        CreateController();
//    }

//    foreach (var player in PhotonNetwork.PlayerList)
//    {
//        Debug.Log($"Player {player.NickName} team: {player.CustomProperties["Team"]}");
//    }
//}

[PunRPC]
    private void SyncTeam(string[] teamData)
    {
        string playerName = teamData[0];
        string team = teamData[1];

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "Team", team }
    };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        Debug.Log($"Synchronized team {team} for player {playerName}.");
    }


}
