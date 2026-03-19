// Ethan Le (3/11/2026): 
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Script handles player's movement and facing direction.
 */

public class Player : MonoBehaviour
{
    public float horizontalSpeed = 3f;
    public float flySpeed = 3f;
    public float fallSpeed = 3f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private bool isFacingLeft = false;
    private bool isGrounded = false; 
    private SpriteRenderer sr; 

    // Assign upward and downward sprites via Unity Inspector: 
    public Sprite upwardSprite;
    public Sprite downwardSprite; 

    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); // Grab the SpriteRenderer component of the player (visual image). 
        rb = GetComponent<Rigidbody2D>(); // Grab the Rigidbody2D component of the player (the player is a Rigidbody2D, and we move this).
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // Need to add a group tag called "Floor" to every GameObject that is a safe floor via Unity Inspector. 
        {
            isGrounded = true; // If player has entered range of a floor, set flag to true. 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // Need to add a group tag called "Floor" to every GameObject that is a safe floor via Unity Inspector. 
        {
            isGrounded = false; // If player has exited range of a floor, set flag to false. 
        }
    }

    /**
     * LEFT arrow key: face left
     * RIGHT arrow key: face right
     * UP arrow key: fly upward
     * DOWN arrow key OR releasing UP: fall downward
     */

    public void OnMove(InputValue value) // Using new Input System:
    {
        moveDirection = value.Get<Vector2>(); // Get movement direction from the Input System.

        Debug.Log(moveDirection);

        // Flip sprite based on horizontal input
        if (moveDirection.x < 0) // Left arrow key pressed.
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip the player horizontally.
            isFacingLeft = true; // Set player to facing left (needed for diagonal movement).
        }
        else if (moveDirection.x > 0) // Right arrow key pressed.
        {
            transform.localScale = new Vector3(1, 1, 1); // Reset the player's facing direction.
            isFacingLeft = false; // Set player to facing right (needed for diagonal movement).
        }
    }

    void FixedUpdate() // FixedUpdate is called at a fixed framerate.
    {
        float xVelocity; // Tracks horizontal speed. 
        float yVelocity; // Tracks vertical speed. 

        // Horizontal movement based on facing direction
        if (isFacingLeft)
        {
            xVelocity = -horizontalSpeed; // If player facing left, move left. 
        }
        else 
        {
            xVelocity = horizontalSpeed; // If player facing right, move right. 
        }

        // Vertical movement
        if (moveDirection.y > 0) // If UP pressed,
        {
            sr.sprite = upwardSprite; // Change the player sprite (on the SpriteRenderer component) to show upward frame. 
            isGrounded = false; // Always set flag to false for whenever player may be on a safe floor and wants to get up.
            yVelocity = flySpeed; // move player up. 
        }
        else if (moveDirection.y < 0) // If DOWN pressed,
        {
            sr.sprite = downwardSprite; // Change the player sprite (on the SpriteRenderer component) to show downward frame. 
            yVelocity = -fallSpeed; // move player down. 
        }
        else // If UP released, 
        {
            sr.sprite = downwardSprite; // Change the player sprite to show downward frame. 
            yVelocity = -fallSpeed; // again, move player down (same as if DOWN pressed). 
        }

        if (isGrounded) // If player is on a safe floor, 
        {
            rb.linearVelocity = Vector2.zero; // stop all movement if player is on a safe floor. 
            return; // Do not continue making a non-vector as we want player to completely stop. 
        }

        rb.linearVelocity = new Vector2(xVelocity, yVelocity); // Create movement non-vector based on assigned x and y velocities. 
    }
}