using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemShadow : MonoBehaviour
{
    public Vector3 shadowScale = Vector3.one;
    public Color shadowColor = Color.black;

    private Sprite gemSprite;
    private SpriteRenderer sr;

    private void Start()
    {
        gemSprite = transform.parent.GetComponent<SpriteRenderer>().sprite;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = gemSprite;
        transform.localScale = shadowScale;
        sr.color = shadowColor;
    }
}
