using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{

    public Text nameText;
    public Text playersInRoomText;
    public string roomName;
    public int playersInRoom;
    public void SetRoom()
    {
        nameText.text = "RoomName: " + roomName;
        playersInRoomText.text ="Players: "+ playersInRoom +"/4";
    }
    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
