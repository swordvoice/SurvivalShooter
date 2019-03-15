using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager manager;
    public bool isInRoom;
    // Start is called before the first frame update
    void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
        else
        {
            if(manager != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonRoom.room.currentScene == MultiplayerSettings.multiplayerSettings.menuScene && isInRoom)
        {
            MainMenuGO.menuGO.mainMenuGO.SetActive(false);
            MainMenuGO.menuGO.roomGO.SetActive(true);
            PhotonRoom.room.ClearPlayerListing();
            PhotonRoom.room.ListPlayer();
            isInRoom = false;
        }
    }
}
