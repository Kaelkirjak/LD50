using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameScene
{

}

public enum SoundType
{
    SFX,
    Music
}

public class GameManager : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Volume Variables

    public Dictionary<SoundType, float> soundVolumes = new Dictionary<SoundType, float>() { { SoundType.Music, 0.5f }, { SoundType.SFX, 0.5f } };
    public Dictionary<SoundType, bool> volumeMutes = new Dictionary<SoundType, bool>() { { SoundType.Music, false}, {SoundType.SFX, false } };

    #endregion Volume Variables

    #region Events

    public event Action<GameScene> onSceneChanged;
    public event Action<SoundType> onVolumeChanged;

    #endregion Events

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Volume Managment

    /// <summary>
    /// Change the volume of a sound type
    /// </summary>
    /// <param name="soundToChange">Sound type to change</param>
    /// <param name="newVolume">New volume</param>
    public void ChangeVolume(SoundType soundToChange, float newVolume)
    {
        soundVolumes[soundToChange] = newVolume;
        onVolumeChanged?.Invoke(soundToChange);
    }

    /// <summary>
    /// Get sound volume
    /// </summary>
    /// <param name="soundToGet">Sound type volume to get</param>
    /// <returns>Volume of the given sound</returns>
    public float GetVolume(SoundType soundToGet)
    {
        if (volumeMutes[soundToGet]){ return 0; }
        return soundVolumes[soundToGet];
    }

    /// <summary>
    /// Toggle mute on a volume type
    /// </summary>
    /// <param name="soundToMute">What type of sound to mute</param>
    public void ToggleVolumeMute(SoundType soundToMute)
    {
        volumeMutes[soundToMute] = !volumeMutes[soundToMute];
        onVolumeChanged?.Invoke(soundToMute);
    }

    #endregion Volume Managment

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Scene Managment

    /// <summary>
    /// Change scene to new one
    /// </summary>
    /// <param name="sceneToChangeTo">Panel to change to</param>
    public void ChangeScene(GameScene sceneToChangeTo)
    {
        onSceneChanged?.Invoke(sceneToChangeTo);
    }

    #endregion Scene Managment

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void Awake()
    {
        InstanceAwake();
    }

    #endregion General Functions

    #region Instance Managemnt

    public static GameManager instance;
    void InstanceAwake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
