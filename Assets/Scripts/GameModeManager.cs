using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{
    public bool gemsLeft = false, jacksLeft = false, timeLeft = false;
    public int timeLeftLimit = 120;
    public int timeLeftAutoGenerateRowInterval = 5;
    public int gemCount, jackCount;
    public float startTime;
    public float currentTimeLeft;
    public Sprite gemIcon, jackIcon, timeIcon;

    private TextMeshProUGUI modeText;
    private Image modeImage;
    private GemGenerator gemGenerator;
    private JackGenerator jackGenerator;
    private SceneLoader sceneLoader;
    private TextMeshProUGUI modeText2;
    private Image modeImage2;


    private void Awake()
    {
        modeText = GameObject.FindGameObjectWithTag("Mode Text").GetComponent<TextMeshProUGUI>();
        modeImage = GameObject.FindGameObjectWithTag("Mode Image").GetComponent<Image>();
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        jackGenerator = GameObject.FindGameObjectWithTag("Jack Generator").GetComponent<JackGenerator>();
        modeText2 = GameObject.FindGameObjectWithTag("Mode Text 2").GetComponent<TextMeshProUGUI>();
        modeImage2 = GameObject.FindGameObjectWithTag("Mode Image 2").GetComponent<Image>();
        modeText2.gameObject.SetActive(false);
        modeImage2.gameObject.SetActive(false);
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        // Set gamemode depending on save singleton gamemode list
        int levelNumber = sceneLoader.GetCurrentSceneIndex() - SaveSingleton.Instance.levelScenesStartIndex;
        SaveSingleton.Gamemode gamemode = SaveSingleton.Instance.levelGamemodes[levelNumber];


        switch (gamemode)
        {
            case SaveSingleton.Gamemode.Gem:
                gemsLeft = true;
                break;
            case SaveSingleton.Gamemode.Jack:
                jacksLeft = true;
                break;
            case SaveSingleton.Gamemode.Timed:
                timeLeft = true;
                break;
            default:
                Debug.LogError("Error");
                break;
        }
    }

    private void Start()
    {
        gemCount = gemGenerator.rowNumber * gemGenerator.columnNumber;
        jackCount = jackGenerator.jackNumber;
        startTime = Time.time;

        if (gemsLeft)
        {
            modeImage.sprite = gemIcon;
            modeText2.gameObject.SetActive(true);
            modeImage2.gameObject.SetActive(true);
        }
        else if (jacksLeft)
            modeImage.sprite = jackIcon;
        else if (timeLeft)
            modeImage.sprite = timeIcon;
    }

    private void Update()
    {
        if (gemsLeft)
        {
            // Limitless: Time
            // Displayed: Gems and Jacks
            modeText.text = gemCount.ToString();
            modeText2.text = jackCount.ToString();
            // Win: Gems = 0
            // Lose: Gems reach bottom or Jacks = 0
            // New Row: After 2 Jacks
        }
        else if (jacksLeft)
        {
            // Limitless: Gems, Time
            // Displayed: Jacks
            modeText.text = jackCount.ToString();
            // Win: Jacks = 0
            // Lose: Gems reach bottom
            // New Row: After 2 Jacks
        }
        else if (timeLeft)
        {
            // Limitless: Gems, Jacks
            // Displayed: Time
            currentTimeLeft = ((int)(timeLeftLimit - (Time.time - startTime)));
            if (currentTimeLeft < 0)
            {
                currentTimeLeft = 0;
                this.enabled = false;
            }
            modeText.text = currentTimeLeft.ToString();
            // Win: Time = 0
            // Lose: Gems reach bottom
            // New Row: After 5 Time
        }
    }
}
