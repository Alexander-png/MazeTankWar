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
            randX = Random.Range(0, lastCellCountinRow);
            randY = Random.Range(0, lastCellCountinColumn);
            cell = MazeCellData.GetCell(GameplayManager.MazeHead, randX, randY, lastCellCountinColumn, lastCellCountinRow);
            randomPickupDataIndex = Random.Range(0, DataPickups.Length);
            randomPickupDataIndex = 2; // Debug
            Instantiate(PickupPrefab, cell.ThisCell.transform).GetComponent<Pickup>().SetPickupData(DataPickups[randomPickupDataIndex]);
        }
    }
}
