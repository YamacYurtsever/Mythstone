using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndInfo : MonoBehaviour
{
    public GameObject starsHolder;
    public GameObject starQuests;

    public void InfoClicked()
    {
        starsHolder.GetComponent<Animator>().SetTrigger("StarsUp");
        starQuests.SetActive(true);

        gameObject.SetActive(false);
    }

    public void StarsBackDownAnimation()
    {
        starsHolder.GetComponent<Animator>().SetTrigger("StarsDown");
    }
}
