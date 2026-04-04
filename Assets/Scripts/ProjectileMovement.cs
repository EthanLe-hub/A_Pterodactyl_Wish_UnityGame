// Ethan Le (4/3/2026):
using UnityEngine; 
using System.Collections; 

/** 
 * Script to control projectile hazards across the screen.
**/
public class ProjectileMovement : MonoBehaviour
{
    public Transform pointA; // One end of the path of the hazard's movement. 
    public Transform pointB; // The other end of the path of the hazard's movement. 
    public float speed = 2f; // Speed at which the hazard moves. 
    public float waitTime = 1f; // Pause for when the hazard reaches one end of its path (before launching again). 

    private Transform target; 
    private bool isWaiting = false; // Flag for when the hazard has stopped at one end of its path (before launching again). 

    void Start()
    {
        target = pointB; // The position where the projectile will end up. 
    }

    /** 
     * Function to move the hazard. 
    **/ 
    void Update()
    {
        if (isWaiting) return; // Do not launch another hazard if it is currently still paused. 

        // Otherwise, move the hazard to the other end (target) at the defined speed: 
        transform.position = Vector3.MoveTowards(
            transform.position, // Go from original position
            target.position, // to the target position. 
            speed * Time.deltaTime
        );

        // If the distance between the hazard and its target position is almost 0, then start coroutine to have hazard reset back to original position and wait before launching again (to simulate a new projectile being launched): 
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            StartCoroutine(WaitAndSwitch());
        }
    }

    /**
     * Function for pausing hazard movement once reaching one end, and defining the other end the hazard will move to next. 
    **/ 
    IEnumerator WaitAndSwitch()
    {
        transform.position = pointA.position; // Reset projectile hazard back to original position (to simulate new projectile about to be launched). 

        isWaiting = true; // Wait before launching the "new" projectile hazard. 

        yield return new WaitForSeconds(waitTime); // Pause the "new" hazard before launching. 

        //target = (target == pointA) ? pointB : pointA;

        isWaiting = false;
    }
}