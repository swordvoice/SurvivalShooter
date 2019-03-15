using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellManager : MonoBehaviour {
    public static int shellNo;
    public int scorePerShell=100;

    int curScore;
    Text text;
    int nextScoreShell;


    void Awake()
    {
        text = GetComponent<Text>();
        shellNo = 0;
        curScore = 0;
        nextScoreShell = scorePerShell;
    }


    void Update()
    {
        
        if (ScoreManager.score >= nextScoreShell && curScore != ScoreManager.score)
        {
            curScore = ScoreManager.score;
            shellNo += 1;
            nextScoreShell += scorePerShell;
        }
        text.text = "SHELL NO: " + shellNo;

    }
}
