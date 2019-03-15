using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PhotonPauseManager : MonoBehaviour
{
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unPaused;
    public GameObject pausePanel;
    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy)
            {
                pausePanel.SetActive(true);
            }
            else
                pausePanel.SetActive(false);
            Pause();
        }
    }
    public void Pause()
    {
        Lowpass();
    }
    public void ReturnToMenu()
    {
        Pause();
    }
    void Lowpass()
    {
        if (Time.timeScale == 0)
        {
            paused.TransitionTo(.01f);
        }
        else
            unPaused.TransitionTo(.01f);
    }
}

