using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField]LayerMask blockMask;
    [SerializeField] AudioPlayer clickPlayer;
    BlockParent connectedBlock;
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Cursor Position

    void SetCursorPosition()
    {
        if(connectedBlock != null)
        {
            connectedBlock.cursorJoint.target = ControlsManager.instance.mouseWorldPosition;
        }
    }
    void MouseClicked(InputAction.CallbackContext ctx)
    {
        CheckForBlocks();
        
    }
    void MouseReleased(InputAction.CallbackContext ctx)
    {
        if (connectedBlock != null)
        {
            connectedBlock.cursorJoint.enabled = false;
        }

    }
    bool CheckForBlocks()
    {
        Collider2D blockCollider = Physics2D.OverlapCircle(ControlsManager.instance.mouseWorldPosition, 0.01f, blockMask);
        if(blockCollider == null)
        {
            return false;
        }
        connectedBlock = blockCollider.GetComponent<BlockParent>();
        Vector2 ancorPoint = Quaternion.Euler(0,0,-blockCollider.transform.eulerAngles.z)*(ControlsManager.instance.mouseWorldPosition - (Vector2)blockCollider.transform.position);
        connectedBlock.cursorJoint.anchor = ancorPoint;
        connectedBlock.cursorJoint.enabled = true;
        connectedBlock.cursorJoint.target = ControlsManager.instance.mouseWorldPosition;
        clickPlayer.PlaySound();
        return true;
    }
    #endregion Cursor Position

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void Start()
    {
        ControlsManager.instance.mousePrimaryClick.started += MouseClicked;
        ControlsManager.instance.mousePrimaryClick.canceled += MouseReleased;
    }

    private void FixedUpdate()
    {
        SetCursorPosition();
    }
    private void OnDestroy()
    {
        ControlsManager.instance.mousePrimaryClick.started -= MouseClicked;
        ControlsManager.instance.mousePrimaryClick.canceled -= MouseReleased;
        
    }

    #endregion General Functions

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}
