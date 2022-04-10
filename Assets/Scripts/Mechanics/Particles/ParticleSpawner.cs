using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Particle Variables

    [Header("Particle Variables")]
    [Tooltip("Particle spawner")][SerializeField]                       ParticleSystem pSystem;
    [Tooltip("Burst particle count")][SerializeField]                   int burstParticleAmount;

    #endregion Particle Variables

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Particle Effects

    /// <summary>
    /// Emit particles in a burst
    /// </summary>
    public void ParticleBurst()
    {
        pSystem.Emit(burstParticleAmount);
    }

    /// <summary>
    /// Emit particles in a burst at a position
    /// </summary>
    /// <param name="position">Where to emit particles at</param>
    public void ParticleBurst(Vector2 position)
    {
        Vector2 startPosition = transform.position;
        transform.position = position;
        pSystem.Emit(burstParticleAmount);
        transform.position = startPosition;
    }

    #endregion Particle Effects

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}
