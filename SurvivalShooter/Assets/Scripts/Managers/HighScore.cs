using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    public static int highScore;
    Text text;
	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
	}
    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore",0);
        text.text = "HighScore: " + highScore;
    }
    private void Update()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        text.text = "HighScore: " + highScore;
    }
}
