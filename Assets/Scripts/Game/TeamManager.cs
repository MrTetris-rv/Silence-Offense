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
        // ��� ������� ��� ������ ���� �� �����, ��� ����� �� �������� ���������
        Instantiate(redTeamPosition, redTeamPosition.transform.position, blueTeamPosition.transform.rotation);
        Instantiate(blueTeamPosition, blueTeamPosition.transform.position, blueTeamPosition.transform.rotation);
    }

    public void SelectTeam(string teamName)
    {
        if (teamSelectionData != null)
        {
            teamSelectionData.selectedTeam = teamName;

            // ��������� ������� ������ ��� �������� ������ (����������)
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", teamName }
        };

            // ������������� �������� ������� ��� ���������� ������
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            // ������ �������� ������� ������ ��� ����� ������, ����� �������� UI ��� ������ ��������
            OnTeamChoiceReady?.Invoke();

            // ������� Hashtable � ������� ������
            ExitGames.Client.Photon.Hashtable playerData = new ExitGames.Client.Photon.Hashtable
        {
            { "playerName", PhotonNetwork.LocalPlayer.NickName },
            { "teamName", teamName }
        };

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncPlayerTeam", RpcTarget.AllBuffered, playerData);

            Destroy(gameObject); // ������� ����� �������, ����� ������� ��� �������
        }
        else
        {
            Debug.LogError("TeamSelectionData is not defined!");
        }
    }


    [PunRPC]
    private void SyncPlayerTeam(ExitGames.Client.Photon.Hashtable playerData)
    {
        // �������� ��� ������ � ������� �� ���������� ������
        string playerName = playerData["playerName"] as string;
        string teamName = playerData["teamName"] as string;

        // ������ �� ����� ������������ ��� ������ ��� ������������� �������
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
