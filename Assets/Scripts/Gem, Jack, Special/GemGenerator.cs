using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemGenerator : MonoBehaviour
{
    private Transform gemStorage;
    private SceneLoader sceneLoader;
    private GameModeManager gameModeManager;

    [Serializable]
    public struct Gem
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float probabilityWeight;
        public bool canNotBeGrouped;
    }
    public List<Gem> gems;

    [Serializable]
    public struct GemClusterSpawnProbabilities
    {
        public int clusterSize;  //whether 3 or 4 gems are being together
        public int minClusterNumber;        //how many number of these 3-4 or more sized clusters have to exist in a map
        public int maxClusterNumber;
        [Range(0f, 100f)] public float probabilityPercentage;           //if its 50% for example you have 50% chance to have another one of the 'clusters' and then it repeats 
    }                                                                   //for another gem cluster but this time its multiplied with itself so its 25%. It goes on forever until it
                                                                        // reacher max amount of clusters.
    public List<GemClusterSpawnProbabilities> gemClusterProbs;

    public float spawnHeight = 5f;
    public float spawnGapX = 1f;
    public float spawnGapY = 1f;
    public int rowNumber = 25;
    public int columnNumber = 5;
    public int screenRowNumber = 9;
    public int startRowNum = 3;
    public bool generatingRow = false;

    private int currentRowNumber = 0;
    private int[,] gemMap;
    private float whileTimer = 0;
    private GameObject gemScoreCanvas;

    private int GetRandomGemID()
    {
        float totalProbabilityWeight = 0;
        foreach(Gem gem in gems)
        {
            totalProbabilityWeight += gem.probabilityWeight;
        }

        float randomNumber = UnityEngine.Random.Range(0, totalProbabilityWeight);

        float currentProbabilityWeight = 0;
        for (int i = 0; i < gems.Count; i++)
        {
            currentProbabilityWeight += gems[i].probabilityWeight;
            if (randomNumber < currentProbabilityWeight)
            {
                return i;
            }
        }

        Debug.LogError("Nothing returned, you fucked up");
        return 0;
    }

    private int GetRandomGroupableGemID()
    {
        float totalProbabilityWeight = 0;
        foreach (Gem gem in gems)
        {
            if(!gem.canNotBeGrouped)
            {
                totalProbabilityWeight += gem.probabilityWeight;
            }
        }

        float randomNumber = UnityEngine.Random.Range(0, totalProbabilityWeight);

        float currentProbabilityWeight = 0;
        for (int i = 0; i < gems.Count; i++)
        {
            if(!gems[i].canNotBeGrouped)
            {
                currentProbabilityWeight += gems[i].probabilityWeight;
                if (randomNumber < currentProbabilityWeight)
                {
                    return i;
                }
            }         
        }

        Debug.LogError("Nothing returned, you fucked up");
        return 0;
    }

    private int[,] GenerateGemMap()
    {
        int[,] newGemMap = new int[rowNumber, columnNumber];

        //set all map id to -1
        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < columnNumber; j++)
            {
                newGemMap[i, j] = -1;            
            }
        }

        foreach(GemClusterSpawnProbabilities gemClusterProb in gemClusterProbs)
        {
            int currentClusterNumber = 0;                            //number of clusters for each gemProbs (each struct which has size min and max number of clusters and probability)
            List<Vector2> gemPositions = new List<Vector2>(0);       //positions of each gem in one cluster
            bool firstPos = true;                                    //first position the code determines when forming the cluster

            whileTimer = 0;
            //forming the minimum number of clusters of size gemClusterProb.clusterSize
            while(currentClusterNumber < gemClusterProb.minClusterNumber)
            {
                whileTimer += Time.deltaTime;
                if (whileTimer > 8f)
                    break;

                Vector2 nextGemPosition = Vector2.zero;
                if(firstPos)
                {
                    int x = UnityEngine.Random.Range(0, rowNumber);
                    int y = UnityEngine.Random.Range(0, columnNumber);
                    nextGemPosition = new Vector2(x, y);
                    firstPos = false;
                }
                else
                {
                    List<Vector2> possibleAdjacentPos = new List<Vector2>(0);

                    foreach (Vector2 pos in gemPositions)
                    {
                        List<Vector2> morePossiblePositions = ListPossibleAdjacentPositions(newGemMap, pos, gemPositions);
                        possibleAdjacentPos.AddRange(morePossiblePositions);
                    }

                    if (possibleAdjacentPos.Count == 0)
                    {
                        firstPos = true;
                        gemPositions.Clear();
                        continue;
                    }
                    else
                    {
                        Vector2 randomPossiblePos = possibleAdjacentPos[UnityEngine.Random.Range(0, possibleAdjacentPos.Count)];
                        nextGemPosition = randomPossiblePos;
                    }
                }

                if (newGemMap[(int) nextGemPosition.x, (int) nextGemPosition.y] == -1)
                {
                    gemPositions.Add(nextGemPosition);
                    if(gemPositions.Count >= gemClusterProb.clusterSize)
                    {
                        int randomClusterableGemID = GetRandomGroupableGemID();
                        foreach (Vector2 gemPosition in gemPositions)
                        {
                            newGemMap[(int) gemPosition.x, (int) gemPosition.y] = randomClusterableGemID;
                        }
                        gemPositions.Clear();
                        firstPos = true;
                        currentClusterNumber++;
                    }
                }
                else
                {
                    firstPos = true;
                    gemPositions.Clear();
                }
            }

            //forming more clusters depending on probability and limiting them to the max number of clusters
            whileTimer = 0;
            int targetClusterNumber = currentClusterNumber;             //set the target number of clusters needed to be formed to current cluster number
                                                                        //and increase by 1 every time the code passes probability check
            float randomPercentage = UnityEngine.Random.Range(0f,100f);
            if (randomPercentage < gemClusterProb.probabilityPercentage)
            {
                targetClusterNumber++;
            }

            while(currentClusterNumber < gemClusterProb.maxClusterNumber && currentClusterNumber < targetClusterNumber)
            {
                whileTimer += Time.deltaTime;
                if (whileTimer > 8f)
                {
                    break;
                }

                Vector2 nextGemPosition = Vector2.zero;
                if (firstPos)
                {
                    int x = UnityEngine.Random.Range(0, rowNumber);
                    int y = UnityEngine.Random.Range(0, columnNumber);
                    nextGemPosition = new Vector2(x, y);
                    firstPos = false;
                }
                else
                {
                    List<Vector2> possibleAdjacentPos = new List<Vector2>(0);

                    foreach (Vector2 pos in gemPositions)
                    {
                        List<Vector2> morePossiblePositions = ListPossibleAdjacentPositions(newGemMap, pos, gemPositions);
                        possibleAdjacentPos.AddRange(morePossiblePositions);
                    }

                    if (possibleAdjacentPos.Count == 0)
                    {
                        firstPos = true;
                        gemPositions.Clear();
                        continue;
                    }
                    else
                    {
                        Vector2 randomPossiblePos = possibleAdjacentPos[UnityEngine.Random.Range(0, possibleAdjacentPos.Count)];
                        nextGemPosition = randomPossiblePos;
                    }
                }

                if (newGemMap[(int)nextGemPosition.x, (int)nextGemPosition.y] == -1)
                {
                    gemPositions.Add(nextGemPosition);
                    if (gemPositions.Count >= gemClusterProb.clusterSize)
                    {
                        int randomClusterableGemID = GetRandomGroupableGemID();
                        foreach (Vector2 gemPosition in gemPositions)
                        {
                            newGemMap[(int)gemPosition.x, (int)gemPosition.y] = randomClusterableGemID;
                        }
                        gemPositions.Clear();
                        firstPos = true;
                        currentClusterNumber++;

                        randomPercentage = UnityEngine.Random.Range(0f, 100f);
                        if (randomPercentage < gemClusterProb.probabilityPercentage)
                        {
                            targetClusterNumber++;
                        }
                    }
                }
                else
                {
                    firstPos = true;
                    gemPositions.Clear();
                }
            }
        }

        //Random Gems for other parts, might have to adjust this so that it cant make clusters bigger
        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < columnNumber; j++)
            {
                if(newGemMap[i, j] == -1)
                {
                    newGemMap[i, j] = GetRandomGemID();
                }
            }
        }

        return newGemMap;
    }

    private List<Vector2> ListPossibleAdjacentPositions(int[,] newGemMap, Vector2 gemPos, List<Vector2> gemPositions)
    {
        List<Vector2> possibleAdjacentPos = new List<Vector2>(0);
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int newX = (int)gemPos.x + dx[i];
            int newY = (int)gemPos.y + dy[i];

            if (newX >= 0 && newX < rowNumber && newY >= 0 && newY < columnNumber)
            {
                Vector2 adjacentVector = new Vector2(newX, newY);
                int adjacentValue = newGemMap[newX, newY];

                if (adjacentValue == -1 && !gemPositions.Contains(adjacentVector))
                    possibleAdjacentPos.Add(adjacentVector);
            }
        }
        
        return possibleAdjacentPos;
    }

    void PrintGemMap()
    {
        int rows = gemMap.GetLength(0);
        int cols = gemMap.GetLength(1);

        string debugtext = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                debugtext += gemMap[i, j] + " ";
            }
            debugtext += "\n";
        }
        Debug.Log(debugtext);
    }

    public void MoveAndGenerateRowsTrigger()
    {
        if (currentRowNumber < rowNumber && !generatingRow)
            StartCoroutine(MoveAndGenerateRowsWaiter());
    }

    IEnumerator MoveAndGenerateRowsWaiter()
    {
        generatingRow = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (gemScoreCanvas.transform.childCount != 0)
            yield return new WaitUntil(() => gemScoreCanvas.transform.childCount == 0);
        MoveAndGenerateRows();
        generatingRow = false;
    }

    private void MoveAndGenerateRows()
    {
        foreach (Transform gemTransform in gemStorage.transform)
        {
            // Check if the gem reached the end
            if (gemTransform.position.y - spawnGapY <= spawnHeight - (screenRowNumber * spawnGapY))
            {
                sceneLoader.LoadLoseScene();
                break;
            }

            // Move Gems
            gemTransform.position = new Vector2(gemTransform.position.x, gemTransform.position.y - spawnGapY);
        }

        // Generate new row
        for (int i = -2; i < 3; i++)
        {
            int gemID = gemMap[currentRowNumber, i + 2];
            GameObject gemPrefab = gems[gemID].prefab;
            if (gemPrefab == null)
            {
                Debug.LogError("No prefab with this id, you fucked up");
            }
            GameObject newGem = Instantiate(gemPrefab, gemStorage);
            newGem.transform.position = new Vector2(spawnGapX * i, spawnHeight);
        }
        currentRowNumber++;
    }

    public bool CheckIfMoreRowsComing()
    {
        if(currentRowNumber < rowNumber)
        {
            return true;
        }
        return false;
    }

    private void Awake()
    {
        gemStorage = GameObject.FindGameObjectWithTag("Gem Storage").transform;
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        gemScoreCanvas = GameObject.FindGameObjectWithTag("Gem Score Canvas");
        gameModeManager = GameObject.FindGameObjectWithTag("Game Mode Manager").GetComponent<GameModeManager>();
    }

    private void Start()
    {
        if (gameModeManager.jacksLeft || gameModeManager.timeLeft)
            rowNumber = 1000;

        gemMap = GenerateGemMap();

        //PrintGemMap();

        for (int i = 0; i < startRowNum; i++)
        {
            MoveAndGenerateRows();
        }

        if (gameModeManager.timeLeft == true)
        {
            StartCoroutine(timeLeftModeAutoGenerate());
        }
    }

    private IEnumerator timeLeftModeAutoGenerate()
    {
        yield return new WaitForSeconds(gameModeManager.timeLeftAutoGenerateRowInterval);
        if (gameModeManager.currentTimeLeft > 0)
        {
            MoveAndGenerateRowsTrigger();
            StartCoroutine(timeLeftModeAutoGenerate());
        }
    }
}