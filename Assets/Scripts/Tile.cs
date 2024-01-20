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
            jackGenerator.ThrowJack(transform.position);
    }
}
