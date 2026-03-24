// Ethan Le (3/15/2026):
using UnityEngine; 

/**
 * Script to have the camera follow the player. 
**/
public class CameraFollow : MonoBehaviour 
{
    public Transform player; // The Player Transform component to follow. 
    public float cameraSpeed = 10f; // Speed at which to move the camera with the player. 

    void LateUpdate()
    {
        if (player == null) 
        {
            return; // Ensure that there exists a player to follow. 
        }

        /* Create a vector based on the player's current position: */ 
        Vector3 targetPosition = new Vector3(
            player.position.x, 
            player.position.y, 
            transform.position.z
        ); 

        /* Camera moves from starting position (transform.position) to the end position (targetPosition) over a certain speed: */ 
        transform.position = Vector3.Lerp(
            transform.position, // Starting position of camera. 
            targetPosition, // End position of camera. 
            cameraSpeed * Time.deltaTime 
        ); 
    }
}