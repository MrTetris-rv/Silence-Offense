using System;
using Photon.Pun;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private TeamSelectionData teamSelectionData;
    [SerializeField] private GameObject redTeamPosition;
    [SerializeField] private GameObject blueTeamPosition;

    public static event Action OnTeamChoiceReady;

    private void Start()
    {
        // Ёти объекты уже должны быть на сцене, нет нужды их повторно создавать
        Instantiate(redTeamPosition, redTeamPosition.transform.position, blueTeamPosition.transform.rotation);
        Instantiate(blueTeamPosition, blueTeamPosition.transform.position, blueTeamPosition.transform.rotation);
    }

    public void SelectTeam(string teamName)
    {
        if (teamSelectionData != null)
        {
            teamSelectionData.selectedTeam = teamName;

            // —охран€ем команду только дл€ текущего игрока (локального)
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", teamName }
        };

            // ”станавливаем свойства команды дл€ локального игрока
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            // “еперь вызываем событие только дл€ этого игрока, чтобы обновить UI или другие элементы
            OnTeamChoiceReady?.Invoke();

            // —оздаем Hashtable с данными игрока
            ExitGames.Client.Photon.Hashtable playerData = new ExitGames.Client.Photon.Hashtable
        {
            { "playerName", PhotonNetwork.LocalPlayer.NickName },
            { "teamName", teamName }
        };

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncPlayerTeam", RpcTarget.AllBuffered, playerData);

            Destroy(gameObject); // ”бираем выбор команды, когда команда уже выбрана
        }
        else
        {
            Debug.LogError("TeamSelectionData is not defined!");
        }
    }


    [PunRPC]
    private void SyncPlayerTeam(ExitGames.Client.Photon.Hashtable playerData)
    {
        // ѕолучаем им€ игрока и команду из переданных данных
        string playerName = playerData["playerName"] as string;
        string teamName = playerData["teamName"] as string;

        // “еперь мы можем использовать эти данные дл€ синхронизации команды
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == playerName)
            {
                ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "Team", teamName }
            };

                player.SetCustomProperties(playerProperties);

                Debug.Log($"Synchronized team {teamName} for player {playerName}.");
                return;
            }
        }
    }
}
