using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonPressText : MonoBehaviour
{
    public RectTransform textTransform;
    public Vector2 upPos, downPos;

    public void AdjustTextDown()
    {
        textTransform.anchoredPosition = downPos;       
    }

    public void AdjustTextUp ()
    {
        textTransform.anchoredPosition = upPos;
    }
}
