using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum LevelState
{
    Default,
    Playing,
    Paused,
    GameOver
}

public class LevelManager : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Level Manager Variables

    [Header("Block spawning variables")]
    [Tooltip("Spawnable blocks")][SerializeField]                       GameObject[] spawnableBlocks;

    [Header("Game variables")]
    [Tooltip("Max difficulty")][SerializeField]                         int maxDifficulty = 10000;
    [Tooltip("Game score")][SerializeField]                             int CurrentScore = 0;
    [Tooltip("Block spawn area")][SerializeField]                       Bounds spawningArea;
    [Tooltip("Amount of blocks spawned")][SerializeField]               Vector2Int blockCount;
    [Tooltip("Spawning frequency curve")][SerializeField]               AnimationCurve spawningFrequencyCurve;
    [Tooltip("Blocks parent gameobject")][SerializeField]               GameObject blocksParentObject;
    [Tooltip("Possible blocks")][SerializeField]                        GameObject[] possibleBlocks;
    public float currentDifficulty
    {
        get
        {
            return 1 / (float)maxDifficulty * (float)currentScore;
        }
    }

    public int currentScore
    {
        get
        {
            return CurrentScore;
        }
        set
        {
            if (value != CurrentScore)
            {
                CurrentScore = value;
                onScoreChanged?.Invoke(currentScore);
            }
        }
    }

    #endregion Level Manager Variables

    #region Level Manager Functionality Variables

    [Tooltip("Block spawner coroutine")]                                Coroutine spawnerCoroutine;

    #endregion Level Manager Functionality Variables

    #region Events

    public event Action<int> onScoreChanged;

    #endregion Events

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region State Managment

    [Tooltip("Current level state")][SerializeField]                    LevelState CurrentState = LevelState.Playing;
    public LevelState currentState
    {
        get
        {
            return CurrentState;
        }
        set
        {
            if(value != CurrentState)
            {
                CurrentState = value;
                onLevelStateChange?.Invoke(CurrentState);
            }
        }
    }
    public event Action<LevelState> onLevelStateChange;

    /// <summary>
    /// Start the game
    /// </summary>
    public void StartLevel()
    {
        currentScore = 0;
        currentState = LevelState.Playing;
        StartSpawner();
    }

    /// <summary>
    /// Called when the plyer loses
    /// </summary>
    public void GameOver()
    {
        currentState = LevelState.GameOver;
        LeaderboardManager.instance.SetScore(currentScore);
        StopSpawner();
    }

    /// <summary>
    /// Called when the player clicks the menu button
    /// </summary>
    public void ReturnToMenu()
    {
        LeaderboardManager.instance.SetScore(currentScore);
        currentState = LevelState.Paused;
        StopSpawner();
    }

    #endregion State Managment

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Block Spawner

    /// <summary>
    /// Start block spawner 
    /// </summary>
    void StartSpawner()
    {
        if (spawnerCoroutine == null)
        {
            spawnerCoroutine = StartCoroutine(BlockSpawner());
        }
    }

    /// <summary>
    /// Stop the block spawner
    /// </summary>
    void StopSpawner()
    {
        if(spawnerCoroutine != null)
        {
            StopCoroutine(spawnerCoroutine);
            spawnerCoroutine = null;
        }
    }

    /// <summary>
    /// Handles block spawning
    /// </summary>
    IEnumerator BlockSpawner()
    {
        while (true)
        {
            int spawnedBlockCount = UnityEngine.Random.Range(blockCount.x, blockCount.y);
            Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(spawningArea.min.x, spawningArea.max.x), UnityEngine.Random.Range(spawningArea.min.y, spawningArea.max.y));
            for (int i = 0; i < spawnedBlockCount; i++)
            {
                GameObject spawnedBlock = Instantiate(possibleBlocks[UnityEngine.Random.Range(0, possibleBlocks.Length)], blocksParentObject.transform);
                spawnedBlock.transform.position = spawnPosition + i * new Vector2(0, 2);
                spawnedBlock.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360f)); ;
            }
            yield return new WaitForSeconds(spawningFrequencyCurve.Evaluate(currentDifficulty));
        }
    }

    #endregion Block Spawner

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void Awake()
    {
        InstanceAwake();
    }

    #endregion General Functions

    #region Instance Managment

    public static LevelManager instance;
    void InstanceAwake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion Instance Managment

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}
