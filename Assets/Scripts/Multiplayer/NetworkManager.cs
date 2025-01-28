using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private byte maxPlayers = 10;
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private Transform playerListContent;
    [SerializeField] private GameObject startGameButton;

    public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    [Inject] MainMenuManager mainMenuManager;

    private void Awake()
    {

        PhotonNetwork.LogLevel = PunLogLevel.Full;
        Debug.Log("Photon LogLevel set to Full");
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        mainMenuManager.OpenMenu("title");
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        if (string.IsNullOrEmpty(roomNameInputField.text)) return;

        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions, TypedLobby.Default);

        mainMenuManager.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        mainMenuManager.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        ClearChildObjects(playerListContent);

        foreach (var player in players)
        {
            var user = Instantiate(playerListItemPrefab, playerListContent);
            user.GetComponent<PlayerListItem>().SetUp(player);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = $"Room creation failed:\n{message}";
            mainMenuManager.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        mainMenuManager.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        mainMenuManager.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
        ClearChildObjects(roomListContent);

        foreach (var roomInfo in cachedRoomList)
        {
            if (cachedRoomList[roomInfo.Key].RemovedFromList) continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomInfo.Value);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        mainMenuManager.OpenMenu("loading");
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach(var info in roomList)
        {
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(info);
            }
        }
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    private void ClearChildObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
