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
    Vector2[] directions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = gameObject.tag;
        string collisionTag = collision.gameObject.tag;

        if (tag == collisionTag || tag == "DinoTooth" || tag == "Lava" || tag == "Super") // Remove After || (Special Gems)) Make it Scalable
        {
            DestroyItandAdjacentGems();
            specialGemSpawner.Trigger(transform.position);
        }
    }

    public void DestroyItandAdjacentGems()
    {
        Destroy(gameObject);
        score.IncreaseScore(gemScore);
        breakCounter.breakCount++;

        foreach (Vector2 direction in directions)
        {
            Vector2 currentPoint = transform.position;
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null)
            {
                if (adjacentCollider.CompareTag(tag))
                    adjacentCollider.gameObject.GetComponent<Gem>().DestroyItandAdjacentGems();
                else
                    adjacentCollider.gameObject.GetComponent<Gem>().CheckAndStartFall();
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
                    connectedGems.Add(adjacentCollider.gameObject.GetComponent<Gem>());
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
                connectedGem.GetComponent<Gem>().DestroyOnFallCheckerStarter();
            }
        }
    }

    private void Awake()
    {
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        breakCounter = GameObject.FindGameObjectWithTag("Break Counter").GetComponent<BreakCounter>();
        specialGemSpawner = GameObject.FindGameObjectWithTag("Special Gem Spawner").GetComponent<SpawnSpecialGems>();
        score = GameObject.FindGameObjectWithTag("Score Text").GetComponent<Score>();

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
        if (transform.position.y < gemGenerator.spawnHeight - (9 * gemGenerator.spawnGapY) + 0.25f)
        {
            Destroy(gameObject);
            score.IncreaseScore(gemScore);
        }
        else
        {
            yield return new WaitForSeconds(Time.deltaTime);
            yield return StartCoroutine(DestroyOnFallChecker());
        }
    }
}
