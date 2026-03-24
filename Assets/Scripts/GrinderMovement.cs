// Ethan Le (3/24/2026):
using UnityEngine; 
using System.Collections; 

/** 
 * Script to control moving hazards across the screen.
**/
public class GrinderMovement : MonoBehaviour
{
    public Transform pointA; // One end of the path of the hazard's movement. 
    public Transform pointB; // The other end of the path of the hazard's movement. 
    public float speed = 2f; // Speed at which the hazard moves. 
    public float waitTime = 1f; // Pause for when the hazard reaches one end of its path. 

    private Transform target; 
    private bool isWaiting = false; // Flag for when the hazard has stopped at one end of its path. 

    void Start()
    {
        target = pointB;
    }

    /** 
     * Function to move the hazard. 
    **/ 
    void Update()
    {
        if (isWaiting) return; // Do not move the hazard if it is currently still paused. 

        // Otherwise, move the hazard to the other end (target) at the defined speed: 
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // If the distance between the hazard and its target position is almost 0, then start coroutine to have hazard wait before moving back to the other side: 
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
        isWaiting = true;

        yield return new WaitForSeconds(waitTime); // Pause the hazard. 

        target = (target == pointA) ? pointB : pointA;

        isWaiting = false;
    }
}