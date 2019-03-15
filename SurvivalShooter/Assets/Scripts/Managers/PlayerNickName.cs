using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNickName : MonoBehaviour
{
    private InputField nickName;
    // Start is called before the first frame update
    void Start()
    {
        nickName = GetComponent<InputField>();
        nickName.text = PlayerPrefs.GetString("PlayerNickName", "");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
