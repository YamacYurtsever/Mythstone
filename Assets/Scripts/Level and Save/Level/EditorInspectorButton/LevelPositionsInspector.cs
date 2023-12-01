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
}
