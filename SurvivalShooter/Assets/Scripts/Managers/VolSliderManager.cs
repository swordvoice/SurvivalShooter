using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolSliderManager : MonoBehaviour {

    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;
    public AudioMixer masterMixer;
	
	// Update is called once per frame
	void Update () {
        float value;
        masterMixer.GetFloat("masterVol", out value);
        masterVolSlider.value = value;
        masterMixer.GetFloat("musicVol", out value);
        musicVolSlider.value = value;
        masterMixer.GetFloat("sfxVol", out value);
        sfxVolSlider.value = value;
    }
}
