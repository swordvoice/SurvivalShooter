using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobby lobby;
    public string roomName;
    public int roomSize=4;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;
    private void Awake()
    {
        lobby = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Server");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListing();
        foreach (RoomInfo room in roomList)
        {
            ListRoom(room);
        }
    }

    void RemoveRoomListing()
    {
        while (roomsPanel.childCount != 0)
        {
            Destroy(roomsPanel.GetChild(0).gameObject);
        }
    }

    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject temListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton temButton = temListing.GetComponent<RoomButton>();
            temButton.roomName = room.Name;
            temButton.playersInRoom = room.MaxPlayers;
            temButton.SetRoom();
        }
    }

    public void CreatRoom()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("There must already be a room with the same name");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are in a Room");
    }
    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }
    public void OnRoomSizeChanged(string sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }
    public void JoinLobbyOnClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    
}
