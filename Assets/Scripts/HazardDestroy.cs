// Ethan Le (4/11/2026):
using UnityEngine;

/** 
 * Script destroys hazards if player reaches safe zone.
**/
public class HazardDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player")) // Check if it was the player who has entered the safe zone. 
        {
            GameObject[] allProjectiles = GameObject.FindGameObjectsWithTag("Projectile"); // Put every GameObject with tag of "Projectile" into the array. 

            foreach (GameObject projectile in allProjectiles) // Loop through each projectile and destroy it. 
            {
                Destroy(projectile);
            }
        }
    }
}