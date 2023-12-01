using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float fallSpeed = 10f;
    public int gemScore = 10;

    private GemGenerator gemGenerator;
    private BreakCounter breakCounter;
    private SpawnSpecialGems specialGemSpawner;
    private Score score;
    private Vector2[] directions;
    private GemScoreDisplayer gemScoreDisplayer;

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
        Destroy(gameObject);
        breakCounter.breakCount++;
        breakCounter.gemScorePositions.Add(transform.position);
        breakCounter.gemScore = gemScore;

        if (transform.position.y > breakCounter.highestGemPos.y && hasConnection())
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
                    adjacentCollider.GetComponent<Gem>().CheckAndStartFall();
            }
        }
    }

    public void DestroyIt()
    {
        Destroy(gameObject);
        score.IncreaseScore(gemScore);
        gemScoreDisplayer.DisplayGemScore(gemScore, transform.position);

        foreach (Vector2 direction in directions)
        {
            Vector2 currentPoint = transform.position;
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null)
            {
                adjacentCollider.GetComponent<Gem>().CheckAndStartFall();
            }
        }
    }

    public void CheckAndStartFall()
    {
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
                connectedGem.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fallSpeed);
                connectedGem.GetComponent<Collider2D>().enabled = false;
                connectedGem.DestroyOnFallCheckerStarter();
            }
        }
    }

    private void Awake()
    {
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        breakCounter = GameObject.FindGameObjectWithTag("Break Counter").GetComponent<BreakCounter>();
        specialGemSpawner = GameObject.FindGameObjectWithTag("Special Gem Spawner").GetComponent<SpawnSpecialGems>();
        score = GameObject.FindGameObjectWithTag("Score Text").GetComponent<Score>();
        gemScoreDisplayer = GameObject.FindGameObjectWithTag("Gem Score Canvas").GetComponent<GemScoreDisplayer>();

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

    private IEnumerator DestroyOnFallChecker()
    {
        if (transform.position.y < gemGenerator.spawnHeight - (gemGenerator.screenRowNumber * gemGenerator.spawnGapY) + 0.5f)
        {
            Destroy(gameObject);
            score.IncreaseScore(gemScore);
            gemScoreDisplayer.DisplayGemScore(gemScore, transform.position);
        }
        else
        {
            yield return new WaitForSeconds(Time.deltaTime);
            yield return StartCoroutine(DestroyOnFallChecker());
        }
    }

    private bool hasConnection()
    {
        foreach (Vector2 direction in directions)
        {
            Vector2 currentPoint = transform.position;
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null && (adjacentCollider.tag != gameObject.tag || adjacentCollider.transform.position.y == gemGenerator.spawnHeight))
                return true;
        }
        return false;
    }
}
