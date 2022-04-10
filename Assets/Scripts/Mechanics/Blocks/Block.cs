using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Block Variables")]
    [Tooltip("Block edge length")][SerializeField]                  float edgeLength = 0.4f;
    [Tooltip("Block neighbors")][SerializeField]                    public List<Block> neighbors = new List<Block>();

    /// <summary>
    /// Get block corners
    /// </summary>
    /// <returns>List of block corners</returns>
    public List<Vector2> GetCorners()
    {
        List<Vector2> corners = new List<Vector2>();
        for (int i = -1; i < 2; i += 2)
        {
            for(int j = -1; j< 2; j += 2){
                corners.Add(new Vector2(transform.localPosition.x + i*edgeLength/2, transform.localPosition.y - j*edgeLength/2));
            }
        }
        return corners;
    }

    /// <summary>
    /// Remove this block from the game
    /// </summary>
    public virtual void RemoveBlock()
    {
        LevelManager.instance.currentScore += 10;
        DustParticleManager.instance.ParticleBurst(transform.position);
        Destroy(gameObject);
    }

    /// <summary>
    /// Remove neighbors from the list of neighbors
    /// </summary>
    /// <param name="neigbhorsToRemove">All the neighbors to remove</param>
    public void RemoveNeighbors(List<Block> neigbhorsToRemove)
    {
        foreach (Block neighbor in neigbhorsToRemove)
        {
            if (neighbors.Contains(neighbor))
            {
                neighbors.Remove(neighbor);
            }
        }
    }
}
