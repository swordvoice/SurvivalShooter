using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    private PhotonPlayerHealth playerHealth;
    private CameraFollow mainCameraFollow;
    private MiniMapCameraMove miniCameraMove;
    private PhotonPlayerShooting photonPlayerShooting;
    private PhotonPlayerMovement photonPlayerMovement;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        GameSetup.GS.reshuffle();  
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerPhoton"),
                GameSetup.GS.playerSpawnPoints[PhotonRoom.room.myNumberInRoom].position,
                GameSetup.GS.playerSpawnPoints[PhotonRoom.room.myNumberInRoom].rotation, 0);
            playerHealth = myAvatar.GetComponent<PhotonPlayerHealth>();
            playerHealth.healthSlider = GameSetup.GS.healthSlider;
            playerHealth.damageImage = GameSetup.GS.damageImage;
            mainCameraFollow = GameSetup.GS.mainCamera.GetComponent<CameraFollow>();
            miniCameraMove = GameSetup.GS.miniMapCamera.GetComponent<MiniMapCameraMove>();
            mainCameraFollow.target = myAvatar.transform;
            miniCameraMove.player = myAvatar;
            photonPlayerMovement = myAvatar.GetComponent<PhotonPlayerMovement>();
            photonPlayerShooting = myAvatar.GetComponentInChildren<PhotonPlayerShooting>();
#if UNITY_ANDROID
            photonPlayerMovement.touchPad = GameSetup.GS.touchPad;
            photonPlayerMovement.turnPad = GameSetup.GS.turnPad;
            photonPlayerShooting.turnPad = GameSetup.GS.turnPad;
            photonPlayerShooting.touchButton = GameSetup.GS.touchButton;
#endif
            
        }
        GameSetup.GS.activePlayer = PhotonRoom.room.playerInGame;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
