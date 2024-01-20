using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class LevelsUnlock : MonoBehaviour
{
    public Transform levelButtonStorage;


    private List<bool> levelUnlocked = new List<bool>();
    private List<int> levelStars = new List<int>();

    [Header("Level Design1 Settings")]
    public Color lockedLevelColorUnityDefault;
    public int unityDefaultColorAlpha = 128;
    public Color lockedLevelColor1;

    [Header("Level Design2 Settings")]
    public Sprite levelLockedSprite;
    public Sprite levelUnlockedSprite;
    public Color lockedLevelColor2;
    private Color fullWhite;

    [Header("Unlock Animation Settings")]
    public float fadeOutDuration = 1.6f;

    [Header("Level Stars Settings")]
    public Sprite starFullSprite;
    public Sprite starEmptySprite;
    public const int starNumber = 3;

    //don't think awake is needed
    /*void Awake()
    {
        LoadLevels();
    }*/


    public void LoadLevels()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        levelUnlocked = data.levelsUnlocked;
        levelStars = data.starsForLevel;

        int lastLevelUnlocked = 0;
        //make locked levels non-interactable
        for (int i = 0; i < levelButtonStorage.childCount; i++)
        {
            Transform starStorageforButton = levelButtonStorage.GetChild(i).GetChild(1);
            if (!levelUnlocked[i])
            {
                levelButtonStorage.GetChild(i).GetComponent<Button>().interactable = false;
                starStorageforButton.gameObject.SetActive(false);
            }
            else
            {
                levelButtonStorage.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();

                //animasyon da olabilir
                //last unlocked level already has animation
                levelButtonStorage.GetChild(i).GetComponent<Button>().interactable = true;
                levelButtonStorage.GetChild(i).GetComponent<Image>().sprite = levelUnlockedSprite;
                lastLevelUnlocked = i + 1;  //1st level is 1

                starStorageforButton.gameObject.SetActive(true);
                int levelStarsNumber = levelStars[i];
                //do all the starshiiet
                for (int j = starNumber - 1; j >= 0; j--)
                {
                    CloseStar(starStorageforButton, j);
                    int x = (int)Mathf.Pow(2, j);
                    if (levelStarsNumber >= x)
                    {
                        levelStarsNumber -= x;
                        OpenStar(starStorageforButton, j);
                    }
                }
            }
        }

        if(lastLevelUnlocked > 1) UnlockAnimation(lastLevelUnlocked);             //not index first level is 1
    }

    private void UnlockAnimation(int level)             //not index first level is 1
    {
        Transform levelButtonObj = levelButtonStorage.GetChild(level - 1);
        Image levelImage = levelButtonObj.GetComponent<Image>();
        levelImage.sprite = levelLockedSprite;
        levelButtonObj.GetComponent<Button>().interactable = false;

        StartCoroutine(FadeLockOut(levelImage));

        Transform starStorageforButton = levelButtonStorage.GetChild(level - 1).GetChild(1);
        int starsNumber = starStorageforButton.childCount;
        for (int i = 0; i < starsNumber; i++)
        {
            Image starImage = starStorageforButton.GetChild(i).GetComponent<Image>();
            starImage.color = new Color(1, 1, 1, 0);
            StartCoroutine(FadeStarsIn(starImage));
        }

        Transform buttonTextTransform = levelButtonStorage.GetChild(level - 1).GetChild(0);
        TextMeshProUGUI buttonText = buttonTextTransform.GetComponent<TextMeshProUGUI>();
        buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, 0);
        StartCoroutine(FadeTextIn(buttonText));
    }

    //Fades the lock sprite out and makes the level interactable and puts (for now null) the unlocked sprite and unlocked level color
    IEnumerator FadeLockOut(Image lockImage)
    {
        float alphaChange = 0.02f / fadeOutDuration;
        
        lockImage.color = new Color(1, 1, 1, lockImage.color.a - alphaChange);
        
        if(lockImage.color.a > 0.05)
        {
            yield return new WaitForSeconds(0.02f);
            yield return StartCoroutine(FadeLockOut(lockImage));
        }
        else
        {
            lockImage.sprite = levelUnlockedSprite;
            lockImage.color = Color.white;
            lockImage.GetComponent<Button>().interactable = true;
        }

    }

    IEnumerator FadeStarsIn(Image starImage)
    {
        float alphaChange = 0.02f / fadeOutDuration;

        starImage.color = new Color(1, 1, 1, starImage.color.a + alphaChange);

        if (starImage.color.a < 0.95)
        {
            yield return new WaitForSeconds(0.02f);
            yield return StartCoroutine(FadeStarsIn(starImage));
        }
        else
        {
            starImage.color = Color.white;
        }

    }

    IEnumerator FadeTextIn(TextMeshProUGUI buttonText)
    {
        float alphaChange = 0.02f / fadeOutDuration;

        buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, buttonText.color.a + alphaChange);

        if (buttonText.color.a < 0.95)
        {
            yield return new WaitForSeconds(0.02f);
            yield return StartCoroutine(FadeTextIn(buttonText));
        }
        else
        {
            buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, 1);
        }

    }

    //Puts yellow filled star that are shown on the bottom of each level
    private void OpenStar(Transform starStorage, int starOrder)
    {
        starStorage.GetChild(starOrder).GetComponent<Image>().sprite = starFullSprite;
    }

    //Puts empty star that are shown on the bottom of each level
    private void CloseStar(Transform starStorage, int starOrder)
    {
        starStorage.GetChild(starOrder).GetComponent<Image>().sprite = starEmptySprite;
    }

    //Level Design that uses only text and no sprite and color and transparency to show unlocked vs locked
    public void LevelDesign1()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        levelUnlocked = data.levelsUnlocked;

        if (lockedLevelColor1.a == 0)
        {
            lockedLevelColor1 = lockedLevelColorUnityDefault;
            lockedLevelColor1.a = unityDefaultColorAlpha;
        }

        for (int i = 0; i < levelButtonStorage.childCount; i++)
        {
            Transform starStorageforButton = levelButtonStorage.GetChild(i).GetChild(1);
            levelButtonStorage.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            levelButtonStorage.GetChild(i).GetComponent<Image>().sprite = null;
            starStorageforButton.gameObject.SetActive(true);
            if (!levelUnlocked[i])
            {
                starStorageforButton.gameObject.SetActive(false);
                levelButtonStorage.GetChild(i).GetComponent<Image>().color = lockedLevelColor1;
            }
        }
    }

    //Level Design that uses locked sprite for locked levels and no sprite and no text for locked levels (assuming unlocked level sprite will come soon)
    public void LevelDesign2()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        levelUnlocked = data.levelsUnlocked;

        fullWhite = Color.white;
        if (lockedLevelColor2.a == 0)
        {
            lockedLevelColor2 = fullWhite;
        }

        for (int i = 0; i < levelButtonStorage.childCount; i++)
        {
            Transform starStorageforButton = levelButtonStorage.GetChild(i).GetChild(1);
            starStorageforButton.gameObject.SetActive(true);
            levelButtonStorage.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            if (!levelUnlocked[i])
            {
                levelButtonStorage.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                starStorageforButton.gameObject.SetActive(false);
                levelButtonStorage.GetChild(i).GetComponent<Image>().sprite = levelLockedSprite;
                levelButtonStorage.GetChild(i).GetComponent<Image>().color = lockedLevelColor2;
            }
            else
            {
                levelButtonStorage.GetChild(i).GetComponent<Image>().sprite = levelUnlockedSprite;
            }
        }
    }
}
