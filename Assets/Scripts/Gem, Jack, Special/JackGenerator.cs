using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackGenerator : MonoBehaviour
{
    [Serializable]
    public struct JackStruct
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float probabilityWeight;
    }
    public List<JackStruct> jacks;

    public float spawnHeight = -5f;
    public float spawnGapX = 1f;
    public float reloadDistance = 1;
    public int jackNumber = 10;
    public int currentJackNumber = 0;
    public float throwSpeed = 1f;
    public int throwsBeforeNewGems = 2;
    public bool flying = false;
    public int maxJacksShown = 2;
    public List<Image> jackImages;

    private float defaultScale;
    private GameObject currentJack = null;
    private GameObject throwingJack = null;
    private int throwNum = 0;
    private BreakCounter breakCounter;
    private GemGenerator gemGenerator;
    private Transform jackStorage;
    private GameObject gemScoreCanvas;
    private GameModeManager gameModeManager;
    //private Pause pause;
    private int x = 0;

    private void MoveAndGenerateJacks()
    {
        currentJack = null;
        foreach (Transform jackTransform in jackStorage.transform)
        {
            Jack jackSc = jackTransform.GetComponent<Jack>();
            SpriteRenderer jackSr = jackTransform.GetComponent<SpriteRenderer>();

            // Remove fifth jack
            if (jackSc.jackorder == 1)
            {
                Destroy(jackTransform.gameObject);
                jackImages[jackSc.jackorder - 1].sprite = null;
                jackImages[jackSc.jackorder - 1].color = new Color(1,1,1,0);
                continue;
            }
            else
            {
                jackImages[jackSc.jackorder - 1].sprite = null;
                jackImages[jackSc.jackorder - 1].color = new Color(1, 1, 1, 0);
                jackSc.jackorder--;
            }

            // Move Jacks
            jackImages[jackSc.jackorder - 1].sprite = jackSr.sprite;
            jackImages[jackSc.jackorder - 1].color = Color.white;

            // Manage size
            if (jackSc.jackorder == 1)
            {
                //jackImages[order].GetComponent<RectTransform>().localScale = new Vector3(defaultScale * 1.5f, defaultScale * 1.5f, 1);
                //jackTransform.localScale = new Vector3(defaultScale * 1.5f, defaultScale * 1.5f, 1);
                currentJack = jackTransform.gameObject;
            }
            else
            {
                //jackTransform.localScale = new Vector3(defaultScale, defaultScale, 1);
                //jackImages[order].GetComponent<RectTransform>().localScale = new Vector3(defaultScale, defaultScale, 1);
            }
        }

        if (currentJackNumber < jackNumber)
        {
            // Generate new jack
            GameObject jackPrefab = jacks[GetRandomJackID()].prefab;
            GameObject newjack = Instantiate(jackPrefab, new Vector2(x, -20), Quaternion.identity,jackStorage);
            x += 1;
            newjack.GetComponent<Jack>().jackorder = maxJacksShown;     //x
            jackImages[maxJacksShown - 1].sprite = newjack.GetComponent<SpriteRenderer>().sprite;               //x
            jackImages[maxJacksShown - 1].color = Color.white;       //x

            currentJackNumber++;
        }
    }

    private int GetRandomJackID()
    {
        float totalProbabilityWeight = 0;
        foreach (JackStruct jack in jacks)
        {
            totalProbabilityWeight += jack.probabilityWeight;
        }

        float randomNumber = UnityEngine.Random.Range(0, totalProbabilityWeight);

        float currentProbabilityWeight = 0;
        for (int i = 0; i < jacks.Count; i++)
        {
            currentProbabilityWeight += jacks[i].probabilityWeight;
            if (randomNumber < currentProbabilityWeight)
            {
                return i;
            }
        }

        Debug.LogError("Nothing returned, you fucked up");
        return 0;
    }

    public void ThrowJack(Vector2 initialPos)
    {
        if (currentJack != null)
        {
            throwingJack = Instantiate(currentJack);
            throwingJack.transform.position = initialPos;
            throwingJack.GetComponent<Rigidbody2D>().velocity = new Vector2(0, throwSpeed);
            currentJack.GetComponent<SpriteRenderer>().enabled = false;
            breakCounter.breakCount = 0;
            flying = true;
        }
    }

    public void jackCrashEventTrigger()
    {
        StartCoroutine(jackCrashEvent());
    }

    private IEnumerator jackCrashEvent()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (gemScoreCanvas.transform.childCount != 0)
            yield return new WaitForSeconds(gemScoreCanvas.GetComponent<GemScoreDisplayer>().fadeOutTime);
        currentJack.GetComponent<SpriteRenderer>().enabled = true;
        MoveAndGenerateJacks();
        throwNum++;
        gameModeManager.jackCount--;
        flying = false;

        if (throwNum >= throwsBeforeNewGems && gameModeManager.timeLeft == false)
        {
            gemGenerator.MoveAndGenerateRowsTrigger();
            throwNum = 0;
        }
    }

    private void Awake()
    {
        breakCounter = GameObject.FindGameObjectWithTag("Break Counter").GetComponent<BreakCounter>();
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        jackStorage = GameObject.FindGameObjectWithTag("Jack Storage").transform;
        //pause = GameObject.FindGameObjectWithTag("Ingame UI Controls").GetComponent<Pause>();
        defaultScale = jacks[0].prefab.transform.localScale.x;
        gemScoreCanvas = GameObject.FindGameObjectWithTag("Gem Score Canvas");
        gameModeManager = GameObject.FindGameObjectWithTag("Game Mode Manager").GetComponent<GameModeManager>();
    }

    private void Start()
    {
        if (gameModeManager.timeLeft)
            jackNumber = 1000;

        for (int i = 0; i < maxJacksShown; i++)
        {
            MoveAndGenerateJacks();
        }
    }
}
