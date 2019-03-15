using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerMovement : MonoBehaviour
{
#if UNITY_ANDROID
    public SimpleTouchPad touchPad;
    public SimpleTurnPad turnPad;
#endif
    private PhotonView PV;
    public float activeTime = 10f;
    private float activedTime = 0f;
    public float speed = 6f;
    private bool powerUp = false;
    Vector3 movement;
    Animator anim;
    Rigidbody PlayerRigidbody;
    int floorMark;
    float camRayLength = 100f;

    private void Awake()
    {
        floorMark = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (powerUp)
        {
            CountPowerUpTime();
        }
    }

    private void FixedUpdate()
    {
        if (PV.IsMine)
        {
            //android control
#if UNITY_ANDROID
        Vector2 direction = touchPad.GetDirection();
        Move(direction.x, direction.y);
            //Animating(direction.x, direction.y);
            PV.RPC("RPC_Animating", RpcTarget.AllBuffered, direction.x, direction.y);
#else
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Move(h, v);
            PV.RPC("RPC_Animating", RpcTarget.AllBuffered, h, v);
            //Animating(h, v);
#endif
            Turning();
        }
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        PlayerRigidbody.MovePosition(transform.position + movement);
    }
    void Turning()
    {

        //android control
#if UNITY_ANDROID
        if (turnPad.GetTouch())
        {
            Vector2 turn = turnPad.GetRotation();
            Vector3 turnRotation = new Vector3(turn.x, 0f, turn.y);
            Quaternion newRotation = Quaternion.LookRotation(turnRotation);
            PlayerRigidbody.MoveRotation(newRotation);
        }
#else
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMark))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            PlayerRigidbody.MoveRotation(newRotation);
        }
#endif
    }
    public bool PowerUpIsActive()
    {
        return powerUp;
    }
    public void ActivePowerUp()
    {
        speed = speed * 1.5f;
        powerUp = true;
    }
    private void DeactivatePowerUp()
    {
        speed = speed / 1.5f;
        powerUp = false;
        activedTime = 0f;
    }
    private void CountPowerUpTime()
    {
        activedTime += Time.deltaTime;
        if (activedTime >= activeTime)
        {
            DeactivatePowerUp();
        }
    }
    [PunRPC]
    void RPC_Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
