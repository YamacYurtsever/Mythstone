using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score = 0;
    public int multiplier = 1;
    public TextMeshProUGUI scoreText;
    public string stringBeforeScore = "";

    public void IncreaseScore(int increment)
    {
        score += increment;
        scoreText.text = stringBeforeScore + score.ToString();
    }
}
