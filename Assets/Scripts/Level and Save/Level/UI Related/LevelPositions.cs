using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPositions : MonoBehaviour
{
    public Vector2 levelsGap = new Vector2(150f, -250f);
    
    private Transform levelStorage;
    private int levelCount;
    private float levelStorageXMax;

    // goes like a spiral staircase
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

    public List<Vector2> levelPositionsList;

    // saves and adjusts level according to positions
    public void SaveLevelPositionsToList()
    {
        levelStorage = transform;
        levelCount = levelStorage.childCount;

        levelPositionsList.Clear();
        for (int i = 0; i < levelCount; i++)
        {
            levelPositionsList.Add(levelStorage.GetChild(i).GetComponent<RectTransform>().anchoredPosition);
        }
    }

    public void AdjustLevelButtonPositionsMethod2()
    {
        levelStorage = transform;
        levelCount = levelStorage.childCount;

        for (int i = 0; i < levelCount; i++)
        {
            Vector2 levelPos = levelPositionsList[i];
            levelStorage.GetChild(i).GetComponent<RectTransform>().anchoredPosition = levelPos;
        }
    }
}