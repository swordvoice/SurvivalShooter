using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public Transform[] playerSpawnPoints;
    public Camera mainCamera;
    public Camera miniMapCamera;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public int activePlayer;
#if UNITY_ANDROID
    public SimpleTurnPad turnPad;
    public SimpleTouchButton touchButton;
    public SimpleTouchPad touchPad;
#endif

    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unPaused;
    public GameObject pausePanel;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }
    public void reshuffle()
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < playerSpawnPoints.Length; t++)
        {
            Transform tmp = playerSpawnPoints[t];
            int r = Random.Range(t, playerSpawnPoints.Length);
            playerSpawnPoints[t] = playerSpawnPoints[r];
            playerSpawnPoints[r] = tmp;
        }
    }
    
    private IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    public void Pause()
    {
        if (!pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
        }
        else
            pausePanel.SetActive(false);
        Lowpass();
    }
    public void ReturnToMenu()
    {
        Pause();
        StartCoroutine(DisconnectAndLoad());
    }
    void Lowpass()
    {
        if (pausePanel.activeInHierarchy)
        {
            paused.TransitionTo(.01f);
        }
        else
            unPaused.TransitionTo(.01f);
    }
}
