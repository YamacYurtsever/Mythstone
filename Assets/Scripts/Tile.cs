using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private JackGenerator jackGenerator;
    private GameObject gemScoreCanvas;

    void Start()
    {
        jackGenerator = GameObject.FindGameObjectWithTag("Jack Generator").GetComponent<JackGenerator>();
        gemScoreCanvas = GameObject.FindGameObjectWithTag("Gem Score Canvas");
    }

    public void ThrowJackInput()
    {
        if (jackGenerator.flying == false && gemScoreCanvas.transform.childCount == 0)
            ThrowJackUITile();  // Use other throw jack method if tile is in world space
    }

    // works if tile was in world space units
    private void ThrowJackWorldSpaceTile()
    {
        jackGenerator.ThrowJack(transform.position);
    }

    // works if tile was in a UI canvas
    private void ThrowJackUITile()
    {
        Vector2 uiPos = GetComponent<RectTransform>().position;


        Vector2 screenPos = uiPos;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        jackGenerator.ThrowJack(worldPos);
    }
}
