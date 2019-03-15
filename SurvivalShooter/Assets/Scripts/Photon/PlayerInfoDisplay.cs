using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
    public Text playerNameText;
    public GameObject playerReadyText;
    public void SetPlayerInfo(string playerName, bool playerReady)
    {
        playerNameText.text = "NickName: " + playerName;
        if (playerReady)
        {
            playerReadyText.SetActive(true);
        }
        else
        {
            playerReadyText.SetActive(false);
        }
    }
}
