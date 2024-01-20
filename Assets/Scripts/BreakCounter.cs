using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCounter : MonoBehaviour
{
    public int breakCount = 0;
    public Vector2 highestGemPos = new Vector2(0, -1000);
    public int gemScore = 0;
    public string gemTag;
    public List<Vector2> gemScorePositions = new List<Vector2>();

    public void ReturnBreakCount() //Debug
    {
        StartCoroutine(ReturnBreakCountCoroutine());
    }

    IEnumerator ReturnBreakCountCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(breakCount);
    }
}
