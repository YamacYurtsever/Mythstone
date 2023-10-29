using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCounter : MonoBehaviour
{
    public int breakCount = 0;

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
