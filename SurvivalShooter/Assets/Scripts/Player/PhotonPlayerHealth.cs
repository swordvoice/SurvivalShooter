using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonPlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    PhotonPlayerMovement playerMovement;
    PhotonPlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    

    Text playerName;
    private PhotonView PV;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PhotonPlayerMovement>();
        playerShooting = GetComponentInChildren<PhotonPlayerShooting>();
        currentHealth = startingHealth;
        damaged = false;
        PV = GetComponent<PhotonView>();
        isDead = false;
        playerName = GetComponentInChildren<Text>();
        Hashtable Dead = new Hashtable();
        Dead.Add("Dead", false);
        PhotonNetwork.SetPlayerCustomProperties(Dead);

    }
    private void Start()
    {
        if (PV.IsMine)
        {
            PV.RPC("RPC_PlayerNickName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }


    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage(int amount)
    {

        if (PV.IsMine)
        {
            damaged = true;

            currentHealth -= amount;

            healthSlider.value = currentHealth;

            playerAudio.Play();
            if (currentHealth <= 0 && !isDead)
            {
                Hashtable Dead = new Hashtable();
                Dead.Add("Dead", true);
                PhotonNetwork.SetPlayerCustomProperties(Dead);
                PV.RPC("RPC_Death", RpcTarget.AllBuffered);
            }
        }
            
    }
    public void DisableControl()
    {
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    [PunRPC]
    void RPC_Death()
    {
        isDead = true;

        playerShooting.DisableEffects();

        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        //this.gameObject.SetActive(false);
        DisableControl();
        PhotonGameOverManager.GO.CheckEndGame();
        //if (ScoreManager.score > HighScore.highScore)
        //{
        //PlayerPrefs.SetInt("HighScore", ScoreManager.score);
        //}

    }
    [PunRPC]
    void RPC_PlayerNickName(string nickName)
    {
        playerName.text = nickName;
    }


    //public void RestartLevel ()
    //{
    //SceneManager.LoadScene (0);
    //}
}
