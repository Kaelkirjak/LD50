using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Controls

    [Header("Game controls")]
    [Tooltip("Mouse position")][SerializeField]             public InputAction mousePosition;
    [Tooltip("Mouse primary click")][SerializeField]        public InputAction mousePrimaryClick;
    [Tooltip("Mouse secondary click")][SerializeField]      public InputAction mouseSecondaryClick;
    [Tooltip("Back button")][SerializeField]                public InputAction backButton;
    public Vector2 mouseWorldPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        }
    }

    #endregion Controls

    #region Controls Enabling

    public void EnableMechanicControls()
    {
        mousePosition.Enable();
        mousePrimaryClick.Enable();
        mouseSecondaryClick.Enable();
    }

    public void DisableMechanicControls()
    {
        mousePosition.Disable();
        mousePrimaryClick.Disable();
        mouseSecondaryClick.Disable();
    }

    public void EnableGeneralControls()
    {
        backButton.Enable();
    }

    public void DisableGeneralControls()
    {
        backButton.Disable();
    }

    #endregion Controls Enabling

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void Start()
    {
        EnableMechanicControls();
        EnableGeneralControls();
    }

    private void Awake()
    {
        InstanceAwake();
    }

    #endregion General Functions

    #region Instance Managment

    public static ControlsManager instance;

    void InstanceAwake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion Instance Managment

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}
