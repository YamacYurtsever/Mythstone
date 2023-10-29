using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpecialGems : MonoBehaviour
{
    public List<GameObject> specialGems;

    private BreakCounter breakCounter;
    private Vector2 gemPos;
    private Transform gemStorage;

    private void Awake()
    {
        breakCounter = GameObject.FindGameObjectWithTag("Break Counter").GetComponent<BreakCounter>();
        gemStorage = GameObject.FindGameObjectWithTag("Gem Storage").transform;
    }

    public void Trigger(Vector2 _gemPos)
    {
        gemPos = _gemPos;
        StartCoroutine(FrameWaiter());
    }

    IEnumerator FrameWaiter()
    {
        yield return new WaitForEndOfFrame();
        GemCombo();
    }

    private void GemCombo()
    {
        int breakCount = breakCounter.breakCount;
        switch (breakCount)
        {
            case 3:
                SpecialGemSpawner(specialGems[0]);
                break;
            case 4:
                SpecialGemSpawner(specialGems[1]);
                break;
            case 5:
                SpecialGemSpawner(specialGems[2]);
                break;
            case 6:
                // 2x
                break;
            case 7:
                // 3x
                break;
            case 8:
                // 4x
                break;
            case 9:
                // 5x
                break;
            default:
                break;
        }
    }

    private void SpecialGemSpawner(GameObject obj)
    {
        GameObject newGem = GameObject.Instantiate(obj);
        newGem.transform.position = gemPos;
        newGem.transform.parent = gemStorage;
    }
}
