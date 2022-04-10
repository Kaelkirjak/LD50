using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock : Block
{
    [Tooltip("Bomb filter")] [SerializeField] ContactFilter2D temp;
    [Tooltip("Destroy radius")] [SerializeField] float destroyRadius = 1f;
    [Tooltip("Explosion delay in seconds")] [SerializeField] float explosionDelay = 0.5f;
    [Tooltip("Audio player for the block")] [SerializeField] AudioPlayer audioPlayer;

    public override void RemoveBlock()
    {
        transform.parent = null;
        gameObject.layer = 1;
        Invoke("Explosion", explosionDelay);
    }

    void Explosion()
    {
        CameraShaker.instance.Shake();
        audioPlayer.PlaySoundOnce();
        LevelManager.instance.currentScore += 50;
        Collider2D[] results = new Collider2D[30];
        int numberOfResultsDestroy = Physics2D.OverlapCircle(transform.position, destroyRadius, temp, results);
        List<GameObject> destroyedBlocks = new List<GameObject>();
        Dictionary<BlockParent, List<Block>> blocksToDestroy = new Dictionary<BlockParent, List<Block>>();

        for (int i = 0; i < numberOfResultsDestroy; i++)
        {
            destroyedBlocks.Add(results[i].gameObject);
        }
        foreach (GameObject currentBlock in destroyedBlocks)
        {
            GameObject currentParentBlock = currentBlock.transform.parent.gameObject;
            Block block = currentBlock.GetComponent<Block>();
            BlockParent parentBlock = currentParentBlock.GetComponent<BlockParent>();
            if (!blocksToDestroy.ContainsKey(parentBlock))
            {
                blocksToDestroy.Add(parentBlock, new List<Block>());
            }
            if (!blocksToDestroy[parentBlock].Contains(block))
            {
                blocksToDestroy[parentBlock].Add(block);
            }
        }

        foreach (BlockParent key in blocksToDestroy.Keys)
        {
            key.BreakBlocks(blocksToDestroy[key]);
        }

        DustParticleManager.instance.ParticleBurst(transform.position);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Invoke("Kill", 1);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
