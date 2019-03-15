using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonLobbyCMM : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobbyCMM lobby;
    public string roomName;
    public Button startButton;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;
    public string playerNickName;
    private List<RoomInfo> roomsList;
    private void Awake()
    {
        lobby = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            startButton.interactable = true;
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        playerNickName = "";
        roomsList = new List<RoomInfo>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.interactable = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //base.OnRoomListUpdate(roomList);
        //RemoveRoomListing();
        int temIndex;
        foreach(RoomInfo room in roomList)
        {
            Debug.Log("OK");
            if(roomsList != null)
            {
                temIndex = roomsList.FindIndex(ByName(room.Name));
            }
            else
            {
                temIndex = -1;
            }
            if(temIndex != -1)
            {
                roomsList.RemoveAt(temIndex);
                Destroy(roomsPanel.GetChild(temIndex).gameObject);
            }
            else
            {
                roomsList.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void RemoveRoomListing()
    {
        for(int i=roomsPanel.childCount-1;i>=0;i--)
        {
            Destroy(roomsPanel.GetChild(i).gameObject);
        }
    }

    void ListRoom(RoomInfo room)
    {
        Debug.Log("ListRoom");
        if(room.IsOpen && room.IsVisible){
            GameObject temListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton temButton = temListing.GetComponent<RoomButton>();
            Debug.Log(room.PlayerCount);
            temButton.roomName = room.Name;
            temButton.playersInRoom = room.PlayerCount;

            temButton.SetRoom();
        }
    }

    public void CreatRoom()
    {
        if (playerNickName == "")
        {
            PhotonNetwork.NickName = "unknow";
        }
        else
            PhotonNetwork.NickName = playerNickName;
        Hashtable Ready = new Hashtable();
        Ready.Add("Ready", true);
        PhotonNetwork.SetPlayerCustomProperties(Ready);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, roomOps);
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("There must already be a room with the same name");
    }

    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn; 
    }
    
    public void JoinLobbyOnClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            if (playerNickName == "")
            {
                PhotonNetwork.NickName = "unknow";
            }
            else
                PhotonNetwork.NickName = playerNickName;
            Hashtable Ready = new Hashtable();
            Ready.Add("Ready", false);
            PhotonNetwork.SetPlayerCustomProperties(Ready);
            PhotonNetwork.JoinLobby();
            Debug.Log("We are in a lobby");
            
        }
    }

    public void OnCancelButtonClicked()
    {
        PhotonNetwork.LeaveLobby();
    }
    public void OnPlayerNickNameChanged(string nickName)
    {
        playerNickName = nickName;
    }
}
