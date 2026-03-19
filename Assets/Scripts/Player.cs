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
    public float flapSpeed = 0.2f; // Time between upward frames. 

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private SpriteRenderer sr; 
    private Animator anim; 
    private bool isFacingLeft = false;
    private bool isGrounded = false; 
    private float flapTimer = 0f; 
    private bool useFirstFrame = true; 

    // Assign upward animation (alternates between upward sprite and downward sprite) and downward sprite via Unity Inspector: 
    public Sprite upwardFrame1;
    public Sprite upwardFrame2; 
    public Sprite downwardSprite;  

    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); // Grab the SpriteRenderer component of the player (visual image). 
        rb = GetComponent<Rigidbody2D>(); // Grab the Rigidbody2D component of the player (the player is a Rigidbody2D, and we move this).
        anim = GetComponent<Animator>(); // Grab the Animator component of the player (renders idle animation). 
    }

    void Update()
    {
        anim.SetBool("isGrounded", isGrounded); // Set the idle animation playing to be true only if the player is on a safe floor. 
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
        anim.enabled = false; // Assume player is not on safe floor every frame. 

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
            flapTimer += Time.deltaTime; // Increment the flapTimer constantly. 

            if (flapTimer >= flapSpeed) // Once the flapTimer has reached 0.5 seconds (same as or greater than flapSpeed),
            {
                flapTimer = 0f; // reset flapTimer back to 0 seconds (so we can reuse this loop).
                useFirstFrame = !useFirstFrame; // Flip the flag (no matter what it is set as): true -> false, false -> true.  
            }

            sr.sprite = useFirstFrame ? upwardFrame1 : upwardFrame2; // Change the player sprite (on the SpriteRenderer component) to show next upward frame in the animation. 
            isGrounded = false; // Always set flag to false for whenever player may be on a safe floor and wants to get up.
            yVelocity = flySpeed; // move player up. 
        }
        else if (moveDirection.y < 0) // If DOWN pressed,
        {
            flapTimer = 0f; // Reset flapTimer back to 0 seconds (so we can reuse UP animation loop cleanly). 
            useFirstFrame = true; // Reset flag back to true to reuse UP animation loop cleanly. 
            sr.sprite = downwardSprite; // Change the player sprite (on the SpriteRenderer component) to show downward frame. 
            yVelocity = -fallSpeed; // move player down. 
        }
        else // If UP released, 
        {
            flapTimer = 0f; // Reset flapTimer back to 0 seconds (so we can reuse UP animation loop cleanly). 
            useFirstFrame = true; // Reset flag back to true to reuse UP animation loop cleanly. 
            sr.sprite = downwardSprite; // Change the player sprite to show downward frame. 
            yVelocity = -fallSpeed; // again, move player down (same as if DOWN pressed). 
        }

        if (isGrounded) // If player is on a safe floor, 
        {
            anim.enabled = true; // Use Animator component when player is on safe floor (to run Idle animation). 
            rb.linearVelocity = Vector2.zero; // stop all movement if player is on a safe floor. 
            return; // Do not continue making a non-vector as we want player to completely stop. 
        }

        rb.linearVelocity = new Vector2(xVelocity, yVelocity); // Create movement non-vector based on assigned x and y velocities. 
    }
}