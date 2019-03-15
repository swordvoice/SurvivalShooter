using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private bool touched;
    // Use this for initialization
    void Awake()
    {
        touched = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        touched = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touched = false;
    }
    public bool GetTouch()
    {
        return touched;
    }

    
}
