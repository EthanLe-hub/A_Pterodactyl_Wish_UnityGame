// Ethan Le (3/11/2026): 
using UnityEngine; 
using UnityEngine.InputSystem; 

/** 
 * Script handles player's movement and facing direction. 
**/ 

public class Player : MonoBehaviour 
{
    public float speed = 5f; 
    private Vector2 moveDirection; 
    private Rigidbody2D rb; 

    /**
     * LEFT arrow key: face left 
       * UP arrow key: fly diagonally up-left
       * Let go of UP arrow key: flydiagonally down-left 
     * RIGHT arrow key: face right 
       * UP arrow key: fly diagonally up-right 
       * Let go of UP arrow key: fly diagonally down-right
    **/ 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Grab the Rigidbody2D component of the player (the player is a Rigidbody2D, and we move this). 
    }

    public void OnMove(InputValue value) // Using new Input System: 
    {
        moveDirection = value.Get<Vector2>(); // Get movement direction from the Input System. 

        // Move the player in the direction of the moveDirection vector. 
        transform.Translate(moveDirection * speed * Time.deltaTime); // Time.deltaTime makes the movement framerate independent. 

        // Update the player's facing direction based on the moveDirection vector. 
        if (moveDirection.x < 0) // Left arrow key pressed.
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip the player horizontally. 
        }

        else if (moveDirection.x > 0) // Right arrow key pressed. 
        {
            transform.localScale = new Vector3(1, 1, 1); // Reset the player's facing direction. 
        }
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate. 
    {
        rb.linearVelocity = moveDirection * speed; // Move the player in the direction of the moveDirection vector. 
    }
    
}