using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPositionsInspector : MonoBehaviour
{
    public LevelPositions levelPositions;

    public void AdjustLevelButtonPositions()
    {
        levelPositions.AdjustLevelButtonPositions();
    }

    public void AdjustLevelButtonPositionsMethod2()
    {
        levelPositions.AdjustLevelButtonPositionsMethod2();
    }

    public void SaveLevelPositionsToList()
    {
        levelPositions.SaveLevelPositionsToList();
    }
}
