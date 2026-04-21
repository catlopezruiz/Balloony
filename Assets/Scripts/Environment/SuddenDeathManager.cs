using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SuddenDeathManager : MonoBehaviour
{
    [Header("References")]
    public Tilemap destructibleTilemap;
    public Tilemap walkableTilemap;
    public GameObject fallingBlockPrefab;

    [Header("Players")]
    public GameObject player1;
    public GameObject player2;

    [Header("Timing")]
    public float delayBeforeSuddenDeath = 30f;
    public float blockSpawnInterval = 0.3f;

    private bool countdownStarted = false;
    private bool suddenDeathStarted = false;

    void Update()
    {
        if (suddenDeathStarted)
            return;

        if (OnlyTwoPlayersRemain() && AllBreakableBlocksGone() && !countdownStarted)
        {
            countdownStarted = true;
            Debug.Log("Sudden Death countdown started!");
            StartCoroutine(StartSuddenDeathAfterDelay());
        }
    }

    bool OnlyTwoPlayersRemain()
    {
        int alivePlayers = 0;

        if (player1 != null && player1.activeInHierarchy)
            alivePlayers++;

        if (player2 != null && player2.activeInHierarchy)
            alivePlayers++;

        return alivePlayers == 2;
    }

    bool AllBreakableBlocksGone()
    {
        if (destructibleTilemap == null)
            return false;

        destructibleTilemap.CompressBounds();
        BoundsInt bounds = destructibleTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (destructibleTilemap.HasTile(pos))
                return false;
        }

        return true;
    }

    IEnumerator StartSuddenDeathAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSuddenDeath);
        Debug.Log("Sudden Death ACTIVATED!");
        suddenDeathStarted = true;
        StartCoroutine(CloseBoardInward());
    }

    IEnumerator CloseBoardInward()
    {
        walkableTilemap.CompressBounds();
        BoundsInt bounds = walkableTilemap.cellBounds;

        int minX = bounds.xMin;
        int maxX = bounds.xMax - 1;
        int minY = bounds.yMin;
        int maxY = bounds.yMax - 1;

        while (minX <= maxX && minY <= maxY)
        {
            List<Vector3Int> ringPositions = new List<Vector3Int>();

            for (int x = minX; x <= maxX; x++)
                ringPositions.Add(new Vector3Int(x, maxY, 0));

            for (int y = maxY - 1; y >= minY; y--)
                ringPositions.Add(new Vector3Int(maxX, y, 0));

            if (minY < maxY)
            {
                for (int x = maxX - 1; x >= minX; x--)
                    ringPositions.Add(new Vector3Int(x, minY, 0));
            }

            if (minX < maxX)
            {
                for (int y = minY + 1; y < maxY; y++)
                    ringPositions.Add(new Vector3Int(minX, y, 0));
            }

            foreach (Vector3Int cellPos in ringPositions)
            {
                SpawnFallingBlock(cellPos);
                yield return new WaitForSeconds(blockSpawnInterval);
            }

            minX++;
            maxX--;
            minY++;
            maxY--;
        }
    }

    void SpawnFallingBlock(Vector3Int cellPos)
    {
        Vector3 worldPos = walkableTilemap.GetCellCenterWorld(cellPos);
        Instantiate(fallingBlockPrefab, worldPos, Quaternion.identity);
    }
}