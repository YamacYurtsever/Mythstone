using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInteractPause : MonoBehaviour
{
    public List<Button> tiles;
    
    public void DisableInteractionTiles()
    {
        foreach (Button tile in tiles)
        {
            tile.interactable = false;
        }
    }
    
    public void EnableInteractionTiles()
    {
        foreach (Button tile in tiles)
        {
            tile.interactable = true;
        }
    }
}
