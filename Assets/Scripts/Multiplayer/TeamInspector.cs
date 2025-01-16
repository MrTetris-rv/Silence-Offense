using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamInspector : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class PlayerTeamInfo
    {
        public string PlayerName;
        public string Team;
    }

    [SerializeField]
    private List<PlayerTeamInfo> playerTeamInfos = new List<PlayerTeamInfo>();

    [SerializeField]
    private TextMeshProUGUI teamDisplayText; // Ссылка на UI-текст

    private void UpdateTeamList()
    {
        playerTeamInfos.Clear();

        foreach (var player in PhotonNetwork.PlayerList)
        {
            string team = "None"; // Если команда не задана
            if (player.CustomProperties.ContainsKey("Team"))
            {
                team = player.CustomProperties["Team"].ToString();
            }

            playerTeamInfos.Add(new PlayerTeamInfo
            {
                PlayerName = player.NickName,
                Team = team
            });
        }

        UpdateUI(); // Обновляем текст UI
    }

    private void UpdateUI()
    {
        if (teamDisplayText == null) return;

        string displayText = "Players and Teams:\n";
        foreach (var info in playerTeamInfos)
        {
            displayText += $"{info.PlayerName}: {info.Team}\n";
        }

        teamDisplayText.text = displayText;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Team"))
        {
            Debug.Log($"Player {targetPlayer.NickName} joined team {changedProps["Team"]}");
            UpdateTeamList();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} joined the room");
        UpdateTeamList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left the room");
        UpdateTeamList();
    }

    private void Start()
    {
        UpdateTeamList();
    }
}
