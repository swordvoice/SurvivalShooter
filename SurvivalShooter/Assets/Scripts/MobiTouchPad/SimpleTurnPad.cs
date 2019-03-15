using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTurnPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float smoothing;

    private Vector2 origin;
    private Vector2 newRotation;
    private Vector2 smoothDirection;
    private bool touched;
    private int pointerID;

    void Awake()
    {
        newRotation = Vector2.zero;
        touched = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!touched)
        {
            touched = true;
            pointerID = data.pointerId;
            origin = data.position;
        }
        //Debug.Log(data);
        //Debug.Log(Input.GetTouch(0));
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == pointerID)
        {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw = currentPosition - origin;
            newRotation = directionRaw.normalized;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId == pointerID)
        {
            newRotation = Vector2.zero;
            touched = false;
        }
    }
    public bool GetTouch()
    {
        return touched;
    }
    public Vector2 GetRotation()
    {
        return newRotation;
    }
}
