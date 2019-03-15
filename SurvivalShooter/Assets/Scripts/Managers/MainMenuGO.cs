using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGO : MonoBehaviour
{
    public static MainMenuGO menuGO;
    public GameObject roomGO;
    public GameObject mainMenuGO;
    // Start is called before the first frame update
    void Awake()
    {
        if(menuGO == null)
        {
            menuGO = this;
        }
        else
        {
            if(menuGO != this)
            {
                Destroy(menuGO.gameObject);
                menuGO = this;
            }
        }
    }

    
}
