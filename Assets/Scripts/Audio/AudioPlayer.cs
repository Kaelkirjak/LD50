using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Audio player variables")]
    [Tooltip("Audio player components")][SerializeField]                                AudioSource audioPlayer;
    [Tooltip("Audio player volume multiplier")][SerializeField]                         float volumeMultiplier;
    [Tooltip("Audio player volume type")][SerializeField]                               SoundType soundType;
    [Tooltip("Use pitch randomizer")][SerializeField]                                   bool pitchRandomizerEnabled;
    [Tooltip("Pitch randomizer range")][SerializeField]                                 float pitchRandomness;

    private void Start()
    {
        ChangeVolume(soundType);
        GameManager.instance.onVolumeChanged += ChangeVolume;
    }

    private void OnDestroy()
    {
        GameManager.instance.onVolumeChanged -= ChangeVolume;
    }

    /// <summary>
    /// Change the volume of the audio source
    /// </summary>
    /// <param name="changedSound">Changed sound type</param>
    void ChangeVolume(SoundType changedSound)
    {
        if(changedSound == soundType)
        {
            audioPlayer.volume = GameManager.instance.GetVolume(soundType) * volumeMultiplier;
        }
    }

    /// <summary>
    /// Randomize the pitch if it is enabled
    /// </summary>
    void RandomizePitch()
    {
        if (pitchRandomizerEnabled)
        {
            audioPlayer.pitch = 1 + Random.Range(-pitchRandomness, pitchRandomness);
        }
    }

    /// <summary>
    /// Play the audio player sound
    /// </summary>
    public void PlaySound()
    {
        RandomizePitch();
        audioPlayer.Play();
    }
    
    /// <summary>
    /// Play sound once
    /// </summary>
    public void PlaySoundOnce()
    {
        RandomizePitch();
        audioPlayer.PlayOneShot(audioPlayer.clip);
    }
}
