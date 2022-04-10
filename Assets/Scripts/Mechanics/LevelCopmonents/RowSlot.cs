using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RowSlot : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Slot Functionality Variables

    [Header("Slot Functionality Variables")]
    [Tooltip("The current GameObject occupying the slot")]              public List<GameObject> currentObject = new List<GameObject>();
    [Tooltip("Layer to check for")][SerializeField]                     int slotDetectionLayerId;

    public bool isSlotOccupied
    {
        get
        {
            return currentObject.Count != 0;
        }
    }

    public event Action onSlotValueChanged;

    #endregion Slot Functionlity Variables

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Slot Functions

    /// <summary>
    /// Check if an object has entered the slot
    /// </summary>
    /// <param name="enteredObject">Object that entered the trigger area</param>
    void CheckSlotFill(GameObject enteredObject)
    {
        if(slotDetectionLayerId != enteredObject.layer) { return; }
        currentObject.Add(enteredObject);
        onSlotValueChanged?.Invoke();
    }

    /// <summary>
    /// Check if an object has left the slot
    /// </summary>
    /// <param name="leftObject">Object that left the trigger area</param>
    void CheckSlotEmpty(GameObject leftObject)
    {
        if (slotDetectionLayerId != leftObject.layer) { return; }
        currentObject.Remove(leftObject);
    }

    #endregion Slot Functions

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckSlotFill(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckSlotEmpty(collision.gameObject);
    }

    #endregion General Functions

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}
