using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool ignoreNextCollision;
    public Rigidbody rb;
    public float impulseForce = 5f;
    private Vector3 startPos;

    public int perfectPass = 0;
    public bool isSuperSpeedActive;


    void Awake()
    {
        startPos = transform.position;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        
        if (ignoreNextCollision)
            return;

        if (isSuperSpeedActive)
            if (!collision.transform.GetComponent<Goal>())
            {
                Destroy(collision.transform.parent.gameObject);
                Debug.Log("Destroying Platform");
            }

            else
            {
                //Adding Restart level functionality via deathpart - Initialized when DeathPart is hit.
                DeathPart deathpart = collision.transform.GetComponent<DeathPart>();

                if (deathpart)
                    deathpart.HitDeathPart();
            }

        //  Only trigger Game Over if the collided object is tagged "DeathPart"
        if (collision.gameObject.CompareTag("DeathPart"))
        {
            Debug.Log("Game Over!");
            // Call GameManager to restart level
            GameManager.singleton.RestartLevel();
            return; // Skip normal bounce
        }

        // Normal bounce behavior
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        // Prevent immediate repeated collisions
        ignoreNextCollision = true;
        Invoke("AllowCollision", .2f);

        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 5, ForceMode.Impulse);
        }
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }
}
