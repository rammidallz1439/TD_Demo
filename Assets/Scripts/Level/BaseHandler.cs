using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vault;

public class BaseHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float InitialYPos;
    public string hexColorCode = "#3A3A3A";
    public Transform SpawnPoint;
    public bool Occupied;
    private void Start()
    {
        InitialYPos = transform.position.y;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Occupied)
            EventManager.Instance.TriggerEvent(new BaseSelectedEvent(this));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }


    public Color GetColor()
    {
        Color newColor;

        if (ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
        {
            return newColor;
        }
        else
        {
            Debug.LogError("Invalid hex Color Code");
            return Color.black;
        }
    }
}
