using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float fallSpeed = 10f;
    public int gemScore = 10;
    public GameObject lavaDrop;

    private Animator animator;
    private GemGenerator gemGenerator;
    private BreakCounter breakCounter;
    private SpawnSpecialGems specialGemSpawner;
    private Score score;
    private Vector2[] directions;
    private GemScoreDisplayer gemScoreDisplayer;
    private GameModeManager gameModeManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = gameObject.tag;
        string collisionTag = collision.gameObject.tag;

        if (tag == collisionTag)
        {
            DestroyGroup();
        }
    }

    public void DestroyGroup()
    {
        breakCounter.highestGemPos = new Vector2(0, -1000);
        DestroyItandAdjacentGems();
        specialGemSpawner.Trigger(gemScore);
    }

    private void DestroyItandAdjacentGems()
    {
        // Applies break animation only to gems with animator (not null) and which have the 'Break' parameter
        if (animator && animator.parameters[0].name == "Break")
        {
            animator.SetTrigger("Break");
            GetComponent<Collider2D>().enabled = false;
        }
        else Destroy(gameObject);
        gameModeManager.gemCount--;
        //Debug.Log(transform.position); //DEBUG
        breakCounter.breakCount++;
        breakCounter.gemScorePositions.Add(transform.position);
        breakCounter.gemScore = gemScore;
        breakCounter.gemTag = gameObject.tag;

        if (transform.position.y > breakCounter.highestGemPos.y && HasConnection())
            breakCounter.highestGemPos = transform.position;

        foreach (Vector2 direction in directions)
        {
            Vector2 currentPoint = transform.position;
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null)
            {
                if (adjacentCollider.CompareTag(tag))
                    adjacentCollider.GetComponent<Gem>().DestroyItandAdjacentGems();
                else
                    adjacentCollider.GetComponent<Gem>().CheckAndStartFallTrigger();
            }
        }
    }

    public void DestroyIt()
    {
        // Doesn't work because gem isn't instantly destroyed, so gems under it don't fall
        /*
        // Applies break animation only to gems with animator (not null) and which have the 'Break' parameter
        if (animator && animator.parameters[0].name == "Break") animator.SetTrigger("Break");
        else Destroy(gameObject);
        */

        // Applies break animation only to gems with animator (not null) and which have the 'Break' parameter
        if (animator && animator.parameters[0].name == "Break")
        {
            animator.SetTrigger("Break");
            GetComponent<Collider2D>().enabled = false;
        }
        else Destroy(gameObject);

        //Destroy(gameObject);
        gameModeManager.gemCount--;
        //Debug.Log(transform.position); //DEBUG
        score.IncreaseScore(gemScore);
        gemScoreDisplayer.DisplayGemScore(gemScore, transform.position, gameObject.tag);

        foreach (Vector2 direction in directions)
        {
            Vector2 currentPoint = transform.position;
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null)
            {
                adjacentCollider.GetComponent<Gem>().CheckAndStartFallTrigger();
            }
        }
    }

    public void DestroyGemFromAnimation()
    {
        Destroy(gameObject);
    }

    public void CheckAndStartFallTrigger()
    {
        StartCoroutine(CheckAndStartFall());
    }

    private IEnumerator CheckAndStartFall()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        List<Vector2> checkedPositions = new List<Vector2>();
        List<Gem> connectedGems = new List<Gem>();
        connectedGems.Add(this);

        bool CheckConnectedGems(Vector2 position)
        {
            checkedPositions.Add(position);

            if (position.y >= gemGenerator.spawnHeight)
                return true;

            bool connectedReachTop = false;

            foreach (Vector2 direction in directions)
            {
                Vector2 adjacentPoint = position + direction;
                Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);

                if (adjacentCollider != null && !checkedPositions.Contains(adjacentPoint))
                {
                    if (!connectedGems.Contains(adjacentCollider.GetComponent<Gem>()))
                        connectedGems.Add(adjacentCollider.GetComponent<Gem>());
                    if (CheckConnectedGems(adjacentPoint))
                        connectedReachTop = true;
                }
            }

            return connectedReachTop;
        }

        if (!CheckConnectedGems(transform.position))
        {
            foreach (Gem connectedGem in connectedGems)
            {
                StartCoroutine(StartFall(connectedGem));
            }
        }
    }

    private IEnumerator StartFall(Gem connectedGem)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (!destroyed && this != null)
        {
            connectedGem.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fallSpeed);
            connectedGem.GetComponent<Collider2D>().enabled = false;
            connectedGem.DestroyOnFallCheckerStarter();
        }
    }

    private void Awake()
    {
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        breakCounter = GameObject.FindGameObjectWithTag("Break Counter").GetComponent<BreakCounter>();
        specialGemSpawner = GameObject.FindGameObjectWithTag("Special Gem Spawner").GetComponent<SpawnSpecialGems>();
        score = GameObject.FindGameObjectWithTag("Score Text").GetComponent<Score>();
        gemScoreDisplayer = GameObject.FindGameObjectWithTag("Gem Score Canvas").GetComponent<GemScoreDisplayer>();
        gameModeManager = GameObject.FindGameObjectWithTag("Game Mode Manager").GetComponent<GameModeManager>();
        animator = GetComponent<Animator>();

        directions = new Vector2[]
        {
            new Vector2(gemGenerator.spawnGapX, 0),
            new Vector2(-gemGenerator.spawnGapX, 0),
            new Vector2(0, gemGenerator.spawnGapY),
            new Vector2(0, -gemGenerator.spawnGapY)
        };
    }

    public void DestroyOnFallCheckerStarter()
    {
        StartCoroutine(DestroyOnFallChecker());
    }

    private bool destroyed = false;
    private IEnumerator DestroyOnFallChecker()
    {
        if (transform.position.y < gemGenerator.spawnHeight - (gemGenerator.screenRowNumber * gemGenerator.spawnGapY) + 0.5f && !destroyed)
        {
            // Spawn lava drop object
            GameObject newLavaDrop = Instantiate(lavaDrop);
            newLavaDrop.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);

            // Delete the gem
            Destroy(gameObject); 
            destroyed = true;
            gameModeManager.gemCount--;
            score.IncreaseScore(gemScore);
            gemScoreDisplayer.DisplayGemScore(gemScore, transform.position, gameObject.tag);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(DestroyOnFallChecker());
        }
    }

    private bool HasConnection()
    {
        Vector2 currentPoint = transform.position;
        foreach (Vector2 direction in directions)
        {
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null && (adjacentCollider.tag != gameObject.tag || adjacentCollider.transform.position.y == gemGenerator.spawnHeight))
                return true;
        }
        return false;
    }
}
