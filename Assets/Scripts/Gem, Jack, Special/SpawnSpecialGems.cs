using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpecialGems : MonoBehaviour
{
    public List<GameObject> specialGems;

    private BreakCounter breakCounter;
    private Transform gemStorage;
    private Score score;
    private GemScoreDisplayer gemScoreDisplayer;
    private GameModeManager gameModeManager;
    private Vector2[] directions;
    private GemGenerator gemGenerator;

    private void Awake()
    {
        breakCounter = GameObject.FindGameObjectWithTag("Break Counter").GetComponent<BreakCounter>();
        gemStorage = GameObject.FindGameObjectWithTag("Gem Storage").transform;
        score = GameObject.FindGameObjectWithTag("Score Text").GetComponent<Score>();
        gemScoreDisplayer = GameObject.FindGameObjectWithTag("Gem Score Canvas").GetComponent<GemScoreDisplayer>();
        gameModeManager = GameObject.FindGameObjectWithTag("Game Mode Manager").GetComponent<GameModeManager>();
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();

        directions = new Vector2[]
{
            new Vector2(gemGenerator.spawnGapX, 0),
            new Vector2(-gemGenerator.spawnGapX, 0),
            new Vector2(0, gemGenerator.spawnGapY),
            new Vector2(0, -gemGenerator.spawnGapY)
        };
    }

    public void Trigger(int gemScore)
    {
        StartCoroutine(GemComboWaiter());
        StartCoroutine(IncreaseGroupScoreWaiter(gemScore));
    }

    IEnumerator GemComboWaiter()
    {
        yield return new WaitForEndOfFrame();
        GemCombo();
        GroupDisplayGemScore();
    }

    private void GemCombo()
    {
        int breakCount = breakCounter.breakCount;
        score.multiplier = 1;
        switch (breakCount)
        {
            case 3:
                int randomIndex = Random.Range(1, 3);
                if (randomIndex == 1)
                    SpecialGemSpawnerTrigger(specialGems[0]);
                else
                    SpecialGemSpawnerTrigger(specialGems[1]);
                break;
            case 4:
                SpecialGemSpawnerTrigger(specialGems[2]);
                break;
            case 5:
                SpecialGemSpawnerTrigger(specialGems[3]);
                break;
            case 6:
                SpecialGemSpawnerTrigger(specialGems[3]);
                score.multiplier = 2;
                break;
            case 7:
                SpecialGemSpawnerTrigger(specialGems[3]);
                score.multiplier = 3;
                break;
            case 8:
                SpecialGemSpawnerTrigger(specialGems[3]);
                score.multiplier = 4;
                break;
            case 9:
                SpecialGemSpawnerTrigger(specialGems[3]);
                score.multiplier = 5;
                break;
            default:
                break;
        }
    }

    private void SpecialGemSpawnerTrigger(GameObject obj)
    {
        StartCoroutine(SpecialGemSpawner(obj));
    }

    private IEnumerator SpecialGemSpawner(GameObject obj)
    {
        yield return new WaitForSeconds(gemScoreDisplayer.GetComponent<GemScoreDisplayer>().fadeOutTime);
        if (HasConnection(breakCounter.highestGemPos))
        {
            GameObject newGem = Instantiate(obj);
            newGem.transform.position = breakCounter.highestGemPos;
            newGem.transform.parent = gemStorage;
            gameModeManager.gemCount++;
        }
    }

    IEnumerator IncreaseGroupScoreWaiter(int gemScore)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        int groupScore = gemScore * breakCounter.breakCount * score.multiplier;
        score.IncreaseScore(groupScore);
    }

    private void GroupDisplayGemScore()
    {
        foreach (Vector2 gemPos in breakCounter.gemScorePositions)
        {
            gemScoreDisplayer.DisplayGemScore(breakCounter.gemScore * score.multiplier, gemPos, breakCounter.gemTag);
        }
        breakCounter.gemScorePositions.Clear();
    }

    private bool HasConnection(Vector2 specialGemPos)
    {
        Vector2 currentPoint = specialGemPos;
        foreach (Vector2 direction in directions)
        {
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null || adjacentPoint.y == gemGenerator.spawnHeight)
                return true;
        }
        return false;
    }
}
