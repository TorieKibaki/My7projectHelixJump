using UnityEngine;

public class DeathPart : MonoBehaviour
{
    private void Awake()
    {
        // Assign the tag automatically so BallController can detect it
        gameObject.tag = "DeathPart";

        // Optional: change color to visualize death parts easily
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = Color.red;
    }

    // Optional helper method, used in BallController if you ever call HitDeathPart()
    public void HitDeathPart()
    {
        Debug.Log("Death Part hit — triggering game over.");
        GameManager.singleton.RestartLevel();
    }
}
