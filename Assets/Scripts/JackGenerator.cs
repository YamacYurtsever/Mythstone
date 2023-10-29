using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackGenerator : MonoBehaviour
{
    public List<GameObject> jacks;
    public float spawnHeight = -5f;
    public float spawnGapX = 1f;
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
            int randomIndex = Random.Range(0, jacks.Count);
            GameObject jackPrefab = jacks[randomIndex];
            GameObject newjack = Instantiate(jackPrefab, jackStorage);
            newjack.transform.position = new Vector2(spawnGapX * 2, spawnHeight);
            currentJackNumber++;
        }
    }

    private void reloadJack()
    {
        if (currentJack != null)
        {
            throwingJack = Instantiate(currentJack);
            throwingJack.transform.position = new Vector2(currentJack.transform.position.x, currentJack.transform.position.y + 1);
            reloaded = true;
        }
    }

    private void throwJack()
    {
        throwingJack.GetComponent<Rigidbody2D>().velocity = new Vector2(0, throwSpeed);
        throwingJack = null;
        breakCounter.breakCount = 0;
    }

    public void jackCrashEvent()
    {
        MoveAndGenerateJacks();
        reloaded = false;
        throwNum++;
    }

    private void Awake()
    {
        breakCounter = GameObject.FindGameObjectWithTag("Break Counter").GetComponent<BreakCounter>();
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        jackStorage = GameObject.FindGameObjectWithTag("Jack Storage").transform;
        defaultScale = jacks[0].transform.localScale.x;
        gemGapX = gemGenerator.spawnGapX;
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

        if (throwNum >= throwsBeforeNewGems)
        {
            gemGenerator.MoveAndGenerateRowsTrigger();
            throwNum = 0;
        }
    }
}
