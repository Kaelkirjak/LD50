using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Row Variables

    [Header("Row variables")]
    [Tooltip("Array of row slots")][SerializeField]                 List<RowSlot> rowSlots;
    [Tooltip("Audio player for the row")][SerializeField]           AudioPlayer audioPlayer;

    #endregion Row Variables

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Row Functions

    /// <summary>
    /// Do all the nessecary for row setup
    /// </summary>
    void StartRow()
    {
        foreach (RowSlot slot in rowSlots)
        {
            slot.onSlotValueChanged += RowSlotChanged;
        }
    }

    /// <summary>
    /// Called when a slot status changes
    /// </summary>
    void RowSlotChanged()
    {
        foreach (RowSlot slot in rowSlots)
        {
            if(!slot.isSlotOccupied)
            {
                return;
            }
        }

        EmptyRowSlots();
    }

    /// <summary>
    /// Make sure deleting row is graceful
    /// </summary>
    void EndRow()
    {
        foreach (RowSlot slot in rowSlots)
        {
            slot.onSlotValueChanged -= RowSlotChanged;
        }
    }

    /// <summary>
    /// Destroy all the blocks in this rows slots
    /// </summary>
    void EmptyRowSlots()
    {
        Dictionary<BlockParent, List<Block>> blocksToDestroy = new Dictionary<BlockParent, List<Block>>();
        foreach (RowSlot slot in rowSlots)
        {
            List<GameObject> currentBlocks = slot.currentObject;
            foreach (GameObject currentBlock in currentBlocks)
            {
                if (currentBlock == null)
                {
                    continue;
                }
                GameObject currentParentBlock = currentBlock.transform.parent.gameObject;
                BlockParent parentBlock = currentParentBlock.GetComponent<BlockParent>();
                Block block = currentBlock.GetComponent<Block>();
                if (!blocksToDestroy.ContainsKey(parentBlock))
                {
                    blocksToDestroy.Add(parentBlock, new List<Block>());
                }
                if (!blocksToDestroy[parentBlock].Contains(block))
                {
                    blocksToDestroy[parentBlock].Add(block);
                }
            }
            slot.currentObject = new List<GameObject>();
        }

        foreach(BlockParent key in blocksToDestroy.Keys)
        {
            key.BreakBlocks(blocksToDestroy[key]);
        }
        CameraShaker.instance.Shake();
        audioPlayer.PlaySound();
    }

    #endregion Row Functions

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void Start()
    {
        StartRow();
    }

    private void OnDestroy()
    {
        EndRow();
    }

    #endregion General Functions

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}
