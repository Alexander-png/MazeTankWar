using MazeWar.Base;
using MazeWar.MazeComponents;
using MazeWar.Pickup;
using MazeWar.Pickup.Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    private GameplayManager GameplayManager;
    private int PossiblePickupCountInTheMaze;
    private int CurrentPickupCountInTheMaze;

    [SerializeField]
    private Pickup PickupPrefab;
    [SerializeField]
    private PickupData[] DataPickups;
    [SerializeField]
    private float PickupSpawnDelay;
    
    private Coroutine PickupSpawningRepeatCoroutine;
        
    public void Init()
    {
        GameplayManager = GlobalManager.GameplayManager;
    }

    public void StartSpawninigPickups()
    {
        PossiblePickupCountInTheMaze = GameplayManager.MazeGenerator.LastCellCountInRow * GameplayManager.MazeGenerator.LastCellCountInColumn - GameplayManager.PlayersAliveCount;
        PickupSpawningRepeatCoroutine = StartCoroutine(SpawnPickupsDelay(PickupSpawnDelay));
    }

    public void StopSpawningPickups()
    {
        if (PickupSpawningRepeatCoroutine != null)
            StopCoroutine(PickupSpawningRepeatCoroutine);
    }

    private IEnumerator SpawnPickupsDelay(float delay)
    {
        int randX;
        int randY;
        int randomPickupDataIndex;
        int lastCellCountinRow = GameplayManager.MazeGenerator.LastCellCountInRow;
        int lastCellCountinColumn = GameplayManager.MazeGenerator.LastCellCountInColumn;

        MazeCellData cell;
        while (true)
        {
            yield return new WaitForSeconds(delay);
            while (true && CurrentPickupCountInTheMaze < PossiblePickupCountInTheMaze)
            {
                randX = Random.Range(0, lastCellCountinRow);
                randY = Random.Range(0, lastCellCountinColumn);
                cell = MazeCellData.GetCell(GameplayManager.MazeHead, randX, randY, lastCellCountinColumn, lastCellCountinRow);
                if (!cell.IsAnyPickupHere && !cell.IsAnyPlayerHere)
                {
                    //randomPickupDataIndex = Random.Range(0, DataPickups.Length);
                    randomPickupDataIndex = 2; // Debug
                    Instantiate(PickupPrefab, cell.ThisCell.transform).GetComponent<Pickup>().SetPickupData(DataPickups[randomPickupDataIndex]);
                    CurrentPickupCountInTheMaze += 1;
                    break;
                }
            }
        }
    }

    public void OnPickupPicked()
    {
        CurrentPickupCountInTheMaze -= 1;
    }
}
