using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonGameOverManager : MonoBehaviour
{
    public static PhotonGameOverManager GO;
    Animator anim;
    public Text gameResultText;
    private bool gameOver;
    private float returnRoomTimer;

    // Start is called before the first frame update
    void Awake()
    {
        if (PhotonGameOverManager.GO == null)
        {
            PhotonGameOverManager.GO = this;
        }
        else
        {
            if (PhotonGameOverManager.GO != this)
            {
                Destroy(PhotonGameOverManager.GO.gameObject);
                PhotonGameOverManager.GO = this;
            }
        }
        anim = GetComponent<Animator>();
        returnRoomTimer = 0;
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (gameOver)
            {
                returnRoomTimer += Time.deltaTime;
                if (returnRoomTimer > 10)
                {
                    PhotonRoom.room.ReturnToRoomMenu();
                }
            }
        }
    }
    // Update is called once per frame

    public void CheckEndGame()
    {
        int activePlayers = 0;
        string winnerNickname = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!(bool)player.CustomProperties["Dead"])
            {
                winnerNickname = player.NickName;
                activePlayers++;
            }
        }
        if (activePlayers == 1)
        {
            gameResultText.text = "Player " + winnerNickname + " Win";
        }
        else if (activePlayers < 1)
        {
            gameResultText.text = "Draw";
        }
        anim.SetTrigger("GameOver");
        gameOver = true;
    }
}
