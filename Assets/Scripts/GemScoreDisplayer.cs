using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemScoreDisplayer : MonoBehaviour
{
    public GameObject gemScoreTextPrefab;
    public float fadeOutTime = 2f;

    public void DisplayGemScore(int gemScore, Vector2 gemPos)
    {
        GameObject gemScoreText = Instantiate(gemScoreTextPrefab, transform);
        gemScoreText.transform.position = gemPos;
        gemScoreText.GetComponent<TextMeshProUGUI>().text = gemScore.ToString();
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
