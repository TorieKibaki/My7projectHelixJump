using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Header("Particle Effect")]
    public ParticleSystem goalParticles; // Assign your particle prefab here
    public float levelTransitionDelay = 0.5f; // Delay before next level

    private bool triggered = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Only trigger once, and only for the ball
        if (triggered)
            return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            triggered = true;

            // Spawn particle effect at goal position
            if (goalParticles != null)
            {
                Instantiate(goalParticles, transform.position, Quaternion.identity);
            }

            // Optional: Play a goal sound here if you want

            // Delay level transition to allow particle effect to play
            Invoke(nameof(LoadNextLevel), levelTransitionDelay);
        }
    }

    private void LoadNextLevel()
    {
        GameManager.singleton.NextLevel();
    }
}
