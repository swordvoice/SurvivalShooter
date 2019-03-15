using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room;
    private PhotonView PV;
    public bool isGameLoaded;
    public int currentScene;

    private Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;
    public int playerInGame;
    public GameObject lobbyGO;
    public GameObject roomGO;
    public GameObject mainMenuGO;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startButton;
    public GameObject readyButton;

    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;


    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }

        }
        DontDestroyOnLoad(this.gameObject);

    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
        {
            Debug.Log(currentScene);
            isGameLoaded = true;
            PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }



    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PhotonNetwork.InRoom)
        {
            mainMenuGO.SetActive(false);
            roomGO.SetActive(true);
            ClearPlayerListing();
            ListPlayer();
            if (PhotonNetwork.IsMasterClient)
            {
                int readyPlayers = 0;
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if ((bool)player.CustomProperties["Ready"])
                    {
                        readyPlayers++;
                    }
                }
                if (readyPlayers == PhotonNetwork.PlayerList.Length)
                {
                    Button start = startButton.GetComponent<Button>();
                    start.interactable = true;
                    //startButton.SetActive(true);
                }
            }
            else
            {
                startButton.SetActive(false);
                readyButton.SetActive(true);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are in a Room");
        lobbyGO.SetActive(false);
        roomGO.SetActive(true);
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
            readyButton.SetActive(true);
        }
        ClearPlayerListing();
        ListPlayer();
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        Debug.Log("Player in room: " + playersInRoom + " Max players: " + MultiplayerSettings.multiplayerSettings.maxPlayer);
        if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

    }
    public void ClearPlayerListing()
    {
        for (int i = playersPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }
    public void ListPlayer()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                PlayerInfoDisplay tempPlayerInfo = tempListing.GetComponent<PlayerInfoDisplay>();
                if (!player.IsMasterClient)
                {
                    tempPlayerInfo.SetPlayerInfo(player.NickName, (bool)player.CustomProperties["Ready"]);
                }
                else
                {
                    tempPlayerInfo.SetPlayerInfo(player.NickName, true);
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player join the room");
        ClearPlayerListing();
        ListPlayer();
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        Debug.Log("Player in room: " + playersInRoom + " Max players: " + MultiplayerSettings.multiplayerSettings.maxPlayer);
        if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }


    // Update is called once per frame

    public void Ready()
    {
        bool ready = (bool)PhotonNetwork.LocalPlayer.CustomProperties["Ready"];
        Debug.Log(ready);
        Hashtable Ready = new Hashtable();
        Ready.Add("Ready", !ready);
        PhotonNetwork.SetPlayerCustomProperties(Ready);
        //if (PV.IsMine)
        //{
        Debug.Log("RunRPC");
        PV.RPC("RPC_Ready", RpcTarget.AllBuffered);
        //}
    }


    public void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.Log("Start Game");
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }

    public void OnQuitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
        lobbyGO.SetActive(true);
        roomGO.SetActive(false);

    }

    [PunRPC]
    private void RPC_Ready()
    {
        Debug.Log("RPC_Ready");
        ClearPlayerListing();
        ListPlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MasterClient");
            int readyPlayers = 0;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if ((bool)player.CustomProperties["Ready"])
                {
                    readyPlayers++;
                }
            }
            Debug.Log("readyplayer " + readyPlayers);
            Debug.Log("playernum " + PhotonNetwork.PlayerList.Length);
            Button start = startButton.GetComponent<Button>();
            if (readyPlayers == PhotonNetwork.PlayerList.Length)
            {
                Debug.Log("All Ready");
                
                start.interactable = true;
                //startButton.SetActive(true);
            }
            else
            {
                start.interactable = false;
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        Debug.Log("LoadedGame");
        playerInGame++;
        Debug.Log("players" + playerInGame);
        Debug.Log("playersroom" + PhotonNetwork.PlayerList.Length);
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            Debug.Log("BeginCreate");
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }
    [PunRPC]
    private void RPC_CreatePlayer()
    {
        Debug.Log("CreatePlayer");
        Vector3 position = transform.position;
        position.x = transform.position.x + (PhotonNetwork.PlayerList.Length - 1) * 2;
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), position, Quaternion.identity, 0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
        playersInRoom--;
        ClearPlayerListing();
        ListPlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            readyButton.SetActive(false);
        }
        //if(currentScene!=MultiplayerSettings.multiplayerSettings.multiplayerScene &&)
    }
    public void ReturnToRoomMenu()
    {
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }
}
