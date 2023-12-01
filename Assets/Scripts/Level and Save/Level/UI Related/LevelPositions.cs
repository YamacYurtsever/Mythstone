using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPositions : MonoBehaviour
{
    public Vector2 levelsGap = new Vector2(150f, -250f);
    
    private Transform levelStorage;
    private int levelCount;
    private float levelStorageXMax;

    public void AdjustLevelButtonPositions()
    {
        levelStorage = transform;
        levelCount = levelStorage.childCount;
        levelStorageXMax = levelStorage.GetComponent<RectTransform>().rect.xMax;

        int toRight = 1;
        for (int i = 1; i < levelCount; i++)
        {
            Vector2 previousPos = levelStorage.GetChild(i - 1).GetComponent<RectTransform>().anchoredPosition;

            float newX = levelsGap.x;
            float number1 = Mathf.Abs(previousPos.x - toRight * levelStorageXMax);
            if (number1 < levelsGap.x)
            {
                newX = levelsGap.x - number1;
                toRight *= -1;
            }

            Vector2 levelPos = previousPos + new Vector2(toRight * newX, levelsGap.y);           

            levelStorage.GetChild(i).GetComponent<RectTransform>().anchoredPosition = levelPos;
        }
    }
}