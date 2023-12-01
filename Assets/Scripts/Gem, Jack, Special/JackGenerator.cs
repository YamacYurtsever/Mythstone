using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackGenerator : MonoBehaviour
{
    [Serializable]
    public struct Jack
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float probabilityWeight;
    }
    public List<Jack> jacks;

    public float spawnHeight = -5f;
    public float spawnGapX = 1f;
    public float reloadDistance = 1;
    public float jackNumber = 10f;
    public float throwSpeed = 1f;
    public int throwsBeforeNewGems = 2;

    private float currentJackNumber = 0;
    private float defaultScale;
    private GameObject currentJack = null;
    private bool reloaded = false;
    private GameObject throwingJack = null;
    private float gemGapX;
    private int throwNum = 0;
    private BreakCounter breakCounter;
    private GemGenerator gemGenerator;
    private Transform jackStorage;
    private GameObject gemScoreCanvas;
    //private Pause pause;

    private void MoveAndGenerateJacks()
    {
        currentJack = null;
        foreach (Transform jackTransform in jackStorage.transform)
        {
            // Remove fifth jack
            if (jackTransform.position.x <= -2 * spawnGapX)
            {
                Destroy(jackTransform.gameObject);
            }

            // Move Jacks
            jackTransform.position = new Vector2(jackTransform.position.x - spawnGapX, jackTransform.position.y);

            // Manage size
            if (jackTransform.position.x == 0)
            {
                jackTransform.localScale = new Vector3(defaultScale * 1.5f, defaultScale * 1.5f, 1);
                currentJack = jackTransform.gameObject;
            }
            else
                jackTransform.localScale = new Vector3(defaultScale, defaultScale, 1);
        }

        if (currentJackNumber < jackNumber)
        {

            // Generate new jack
            GameObject jackPrefab = jacks[GetRandomJackID()].prefab;
            GameObject newjack = Instantiate(jackPrefab, jackStorage);
            newjack.transform.position = new Vector2(spawnGapX * 2, spawnHeight);
            currentJackNumber++;
        }
    }

    private int GetRandomJackID()
    {
        float totalProbabilityWeight = 0;
        foreach (Jack jack in jacks)
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


    private void reloadJack()
    {
        if (currentJack != null)
        {
            throwingJack = Instantiate(currentJack);
            throwingJack.transform.position = new Vector2(currentJack.transform.position.x, currentJack.transform.position.y + reloadDistance);
            currentJack.GetComponent<SpriteRenderer>().enabled = false;
            reloaded = true;
        }
    }

    private void throwJack()
    {
        throwingJack.GetComponent<Rigidbody2D>().velocity = new Vector2(0, throwSpeed);
        throwingJack = null;
        breakCounter.breakCount = 0;
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
        reloaded = false;
        throwNum++;

        if (throwNum >= throwsBeforeNewGems)
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
        gemGapX = gemGenerator.spawnGapX;
        gemScoreCanvas = GameObject.FindGameObjectWithTag("Gem Score Canvas");
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            MoveAndGenerateJacks();
        }
    }

    private void Update()
    {      
        if (Input.GetMouseButtonDown(0))
        {
            if (!reloaded)
                reloadJack();
            else if (throwingJack != null)
                throwJack();
        }

        if (throwingJack != null)
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && throwingJack.transform.position.x > (gemGapX * -2))
            {
                throwingJack.transform.position = new Vector2(throwingJack.transform.position.x - gemGapX, throwingJack.transform.position.y);
            }
            else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && throwingJack.transform.position.x < (gemGapX * 2))
            {
                throwingJack.transform.position = new Vector2(throwingJack.transform.position.x + gemGapX, throwingJack.transform.position.y);
            }
        }
    }
}
