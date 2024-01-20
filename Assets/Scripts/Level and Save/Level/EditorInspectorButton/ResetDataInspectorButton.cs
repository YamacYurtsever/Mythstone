using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataInspectorButton : MonoBehaviour
{
    public ResetData resetData;

    public void ResetScoreDataforLevel()
    {
        resetData.ResetScoreDataforLevel();
    }

    public void ResetStarDataCompletelyforLevel()
    {
        resetData.ResetStarDataCompletelyforLevel();
    }

    public void ResetCertainStarDataforLevel()
    {
        resetData.ResetCertainStarDataforLevel();
    }
}
