using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockParent : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    [Header("Block Parent Variables")]
    [Tooltip("Child blocks")] [SerializeField] List<Block> childBlocks = new List<Block>();
    [Tooltip("Block collider")] [SerializeField] PolygonCollider2D blockCollider;
    [Tooltip("Block edge length")] [SerializeField] int edgeLength = 4;
    [Tooltip("Empty prefab")] [SerializeField] GameObject emptyPrefab;
    public TargetJoint2D cursorJoint;

    [Header("Generation")]
    [Tooltip("Chance of bomb spawning in block (1/x)")] [SerializeField] int bombSpawnChance = 10;
    [Tooltip("Bomb prefab")] [SerializeField] GameObject bombPrefab;


    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Block Collider Setting

    /// <summary>
    /// Get all the corners of the child boxes
    /// </summary>
    /// <returns>List of child corners</returns>
    List<Vector2Int> GetChildCorners()
    {
        List<Vector2Int> corners = new List<Vector2Int>();
        List<Vector2Int> allCorners = new List<Vector2Int>();
        foreach (Block block in childBlocks)
        {
            List<Vector2> blockCorners = block.GetCorners();
            foreach (Vector2 corner in blockCorners)
            {
                allCorners.Add(new Vector2Int(Mathf.RoundToInt(corner.x * 10), Mathf.RoundToInt(corner.y * 10)));
                if (!corners.Contains(new Vector2Int(Mathf.RoundToInt(corner.x * 10), Mathf.RoundToInt(corner.y * 10))))
                {
                    corners.Add(new Vector2Int(Mathf.RoundToInt(corner.x * 10), Mathf.RoundToInt(corner.y * 10)));
                }
            }
        }

        Vector2Int mostFreq = allCorners.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
        int count = allCorners.Where(s => s != null && s.Equals(mostFreq)).Count();
        if (count == 4)
        {
            corners.Remove(mostFreq);
        }
        return corners;
    }

    /// <summary>
    /// Sort the corners so they make a clockwise polygon collider
    /// </summary>
    /// <param name="corners">Corners to sort</param>
    /// <returns>List of corners that make a polygon collider</returns>
    List<Vector2Int> SortCorners(List<Vector2Int> corners)
    {
        List<Vector2Int> sortedCorners = new List<Vector2Int>();

        Vector2Int startCorner = corners[0];

        for (int i = 0; i < corners.Count; i++)
        {
            if (corners[i].y < startCorner.y)
            {
                startCorner = corners[i];
            }
        }

        for (int i = 0; i < corners.Count; i++)
        {
            if (corners[i].y == startCorner.y && corners[i].x < startCorner.x)
            {
                startCorner = corners[i];
            }
        }

        sortedCorners.Add(startCorner);

        while (true)
        {
            if (sortedCorners.Count == corners.Count)
            {
                break;
            }

            Vector2Int currentCorner = sortedCorners[sortedCorners.Count - 1];
            Vector2Int nextBlock = FindNextBlock(sortedCorners, currentCorner, corners);
            sortedCorners.Add(nextBlock);
        }
        return sortedCorners;
    }

    /// <summary>
    /// Find the next corner in the polygon
    /// </summary>
    /// <param name="temp"></param>
    /// <param name="currentCorner"></param>
    /// <param name="corners"></param>
    /// <returns></returns>
    Vector2Int FindNextBlock(List<Vector2Int> temp, Vector2Int currentCorner, List<Vector2Int> corners)
    {
        Vector2Int up = new Vector2Int(currentCorner.x, currentCorner.y - edgeLength);
        Vector2Int right = new Vector2Int(currentCorner.x + edgeLength, currentCorner.y);
        Vector2Int down = new Vector2Int(currentCorner.x, currentCorner.y + edgeLength);
        Vector2Int left = new Vector2Int(currentCorner.x - edgeLength, currentCorner.y);

       
        if (corners.Contains(right) && !temp.Contains(right))
        {
            return corners[corners.IndexOf(right)];
        }
        else if (corners.Contains(down) && !temp.Contains(down))
        {
            return corners[corners.IndexOf(down)];
        }
        else if (corners.Contains(left) && !temp.Contains(left))
        {
            return corners[corners.IndexOf(left)];
        }
        else if (corners.Contains(up) && !temp.Contains(up))
        {
            return corners[corners.IndexOf(up)];
        }
        return new Vector2Int();
    }

    /// <summary>
    /// Set the polygon collider to represent the blocks correctly
    /// </summary>
    void SetCollider()
    {
        List<Vector2Int> sortedCorners = SortCorners(GetChildCorners());
        Vector2[] vectorSortedCorners = new Vector2[sortedCorners.Count];

        for (int i = 0; i < sortedCorners.Count; i++)
        {
            vectorSortedCorners[i] = (Vector2)sortedCorners[i] / 10;
        }

        blockCollider.points = vectorSortedCorners;
    }

    #endregion Block Collider Setting

    #region Block Breaking Functions

    /// <summary>
    /// Remove blocks from the current list of children blocks
    /// </summary>
    /// <param name="blocksToBreak">List of blocks to break</param>
    public void BreakBlocks(List<Block> blocksToBreak)
    {
        foreach (Block block in blocksToBreak)
        { 
            childBlocks.Remove(block);
            block.RemoveBlock();
        }
        foreach (Block block in childBlocks)
        {
            block.RemoveNeighbors(blocksToBreak);
        }
        
        List<Block> firstGroup = CheckForSplitting();
        if (firstGroup.Count == 0)
        {
            return;
        }
        MakeNewBlock(firstGroup);
        SetCollider();
    }

    /// <summary>
    /// Check if child blocks have been split into 2 bigger objects
    /// </summary>
    /// <returns></returns>
    List<Block> CheckForSplitting()
    {
        if (childBlocks.Count > 0)
        {
            List<Block> checkedBlocks = new List<Block>();
            Queue<Block> waitingBlocks = new Queue<Block>();

            waitingBlocks.Enqueue(childBlocks[0]);

            while (true)
            {
                if (waitingBlocks.Count == 0)
                {
                    break;
                }

                Block block = waitingBlocks.Dequeue();
                checkedBlocks.Add(block);

                foreach (Block neighbor in block.neighbors)
                {
                    if (!checkedBlocks.Contains(neighbor) && (!waitingBlocks.Contains(neighbor)))
                    {
                        waitingBlocks.Enqueue(neighbor);
                    }
                }
            }

            return checkedBlocks;

        } 
        else
        {
            Destroy(gameObject);
            return new List<Block>();
        }
    }

    void MakeNewBlock(List<Block> firstFormation)
    {
        if (firstFormation.Count < childBlocks.Count)
        {
            Vector2 arithmeticMiddle = new Vector2();
            int sum = 0;

            List<Block> newFormation = new List<Block>();

            foreach (Block block in childBlocks)
            {
                if (!firstFormation.Contains(block))
                {
                    sum++;
                    arithmeticMiddle += (Vector2) block.gameObject.transform.position;
                    newFormation.Add(block);
                }
            }
            foreach (Block block in newFormation)
            {
                childBlocks.Remove(block);
            }

            arithmeticMiddle /= sum;

            GameObject newParent = Instantiate(emptyPrefab);
            newParent.transform.position = arithmeticMiddle;
            newParent.transform.rotation = transform.rotation;

            foreach (Block block in newFormation)
            {
                block.gameObject.transform.SetParent(newParent.transform);
            }

            BlockParent newBlockParent = newParent.GetComponent<BlockParent>();
            newBlockParent.setChildren(newFormation);
        }
    }

    public void setChildren(List<Block> children)
    {
        childBlocks = children;
        //List<Block> newFormation = CheckForSplitting();
        //MakeNewBlock(newFormation);
        SetCollider();
    }

    #endregion Block Breaking Functions

    #region Level State

    void LevelReset(LevelState newState)
    {
        if (newState == LevelState.Playing)
        {
            Destroy(gameObject);
        }
    }

    #endregion Level State

    #region Bomb

    void TrySpawnBomb()
    {
        if(Random.Range(0, bombSpawnChance) == 0)
        {
            int bombLocation = Random.Range(0, childBlocks.Count);

            GameObject bombObject = Instantiate(bombPrefab, transform);
            bombObject.transform.position = childBlocks[bombLocation].transform.position;
            bombObject.transform.rotation = childBlocks[bombLocation].transform.rotation;

            BombBlock bombScript = bombObject.GetComponent<BombBlock>();
            bombScript.neighbors = childBlocks[bombLocation].neighbors;

            foreach (Block block in childBlocks[bombLocation].neighbors)
            {
                block.neighbors.Remove(childBlocks[bombLocation]);
                block.neighbors.Add(bombScript);
            }
            Destroy(childBlocks[bombLocation].gameObject);
            childBlocks[bombLocation] = bombScript;
        }
    }

    #endregion Bomb

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void Start()
    {
        if(transform.parent != null)
        {
            TrySpawnBomb();
        }
        SetCollider();
        LevelManager.instance.onLevelStateChange += LevelReset;
    }

    private void OnDestroy()
    {
        LevelManager.instance.onLevelStateChange -= LevelReset;
    }

    #endregion General Functions

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}