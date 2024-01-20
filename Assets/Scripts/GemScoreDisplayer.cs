using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemScoreDisplayer : MonoBehaviour
{
    public GameObject gemScoreTextPrefab;
    public float fadeOutTime = 2f;

    public Color amethystColor;
    public Color diamondColor;
    public Color emeraldColor;
    public Color rubyColor;
    public Color sapphireColor;
    public Color topazColor;
    public Color coalColor;
    public Color dinoToothColor;
    public Color lavaColor;
    public Color superColor;

    public void DisplayGemScore(int gemScore, Vector2 gemPos, string gemTag)
    {
        GameObject gemScoreText = Instantiate(gemScoreTextPrefab, transform);
        gemScoreText.transform.position = gemPos;
        gemScoreText.GetComponent<TextMeshProUGUI>().text = gemScore.ToString();

        switch (gemTag)
        {
            case "Amethyst":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = amethystColor;
                break;
            case "Diamond":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = diamondColor;
                break;
            case "Emerald":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = emeraldColor;
                break;
            case "Ruby":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = rubyColor;
                break;
            case "Sapphire":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = sapphireColor;
                break;
            case "Topaz":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = topazColor;
                break;
            case "Coal":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = coalColor;
                break;
            case "Dino Tooth":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = dinoToothColor;
                break;
            case "Lava":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = lavaColor;
                break;
            case "Super":
                gemScoreText.GetComponent<TextMeshProUGUI>().color = superColor;
                break;
            default:
                break;
        }

        StartCoroutine(FadeOut(gemScoreText));
    }

    private IEnumerator FadeOut(GameObject gemScoreText)
    {
        float alp = 1f;
        Color textColor = gemScoreText.GetComponent<TextMeshProUGUI>().color;
        while (alp > 0)
        {
            alp -= 0.01f;
            textColor.a = alp;
            gemScoreText.GetComponent<TextMeshProUGUI>().color = textColor;
            yield return new WaitForSeconds(fadeOutTime / 100);
        }
        Destroy(gemScoreText);
    }
}
