// Ethan Le (4/11/2026):
using UnityEngine;

/**
 * Script to handle position of player spawn (for whenever the Game Scene loads):
**/
public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Get the Player GameObject via its Tag.

        if (player != null) // Safety check.
        {
            // Get the position of the PlayerSpawner GameObject and have the Player GameObject be at that position:
            player.transform.position = transform.position; 

            // Get the Rigidbody2D component of the Player GameObject: 
            Rigidbody2D rbPlayer = player.GetComponent<Rigidbody2D>(); 

            if (rbPlayer != null) // Safety check. 
            {
                rbPlayer.linearVelocity = Vector2.zero; // Stop all movement if player is on a safe floor. 
                rbPlayer.angularVelocity = 0f; 
            }
        }
    }
}