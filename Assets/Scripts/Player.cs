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
    //public float flapSpeed = 0.2f; // Time between upward frames. 

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private SpriteRenderer sr; 
    private Animator anim; // Holds Animator component which links to the Animation asset that has the 3 frames of flying animation (the key frames). 
    private bool isFacingLeft = false;
    private bool isGrounded = false; 
    private float flapTimer = 0f; 
    private bool useFirstFrame = true; 

    // Assign glidingSprite and restingSprite via Unity Inspector: 
    public Sprite glidingSprite;
    public Sprite restingSprite;  

    public ScoreManager scoreManager; // Assign GameObject with ScoreManager script via Unity inspector. 

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
        // The floors/walls have Box Collider 2D components, but only the Floors are safe for landing. 
        if (collision.gameObject.CompareTag("Floor")) // Need to add a group tag called "Floor" to every GameObject that is a safe floor via Unity Inspector. 
        {
            isGrounded = true; // If player has entered range of a floor, set flag to true. 
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        // For coins, it has a Circle Collider 2D component. 
        if (collision.CompareTag("Coin")) // For when the player comes into contact with a GameObject with the tag "Coin".
        {
            scoreManager.collectCoin(); // If the player touches a valid coin, increase score and player speed. 

            Destroy(collision.gameObject); // Destroy the coin to make it disappear from the screen. 
        }
    }

    /*void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // Need to add a group tag called "Floor" to every GameObject that is a safe floor via Unity Inspector. 
        {
            isGrounded = false; // If player has exited range of a floor, set flag to false. 
        }
    }*/ 

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
            xVelocity = -horizontalSpeed * scoreManager.getSpeed(); // If player facing left, move left. 
        }
        else 
        {
            xVelocity = horizontalSpeed * scoreManager.getSpeed(); // If player facing right, move right. 
        }

        // Vertical movement
        if (moveDirection.y > 0) // If UP pressed,
        {
            flapTimer += Time.deltaTime; // Increment the flapTimer constantly. 

            /*if (flapTimer >= flapSpeed) // Once the flapTimer has reached 0.5 seconds (same as or greater than flapSpeed),
            {
                flapTimer = 0f; // reset flapTimer back to 0 seconds (so we can reuse this loop).
                //useFirstFrame = !useFirstFrame; // Flip the flag (no matter what it is set as): true -> false, false -> true.  
            }*/

            //sr.sprite = useFirstFrame ? upwardFrame1 : upwardFrame2; // Change the player sprite (on the SpriteRenderer component) to show next upward frame in the animation. 
            anim.enabled = true; // Use Animator component when player is flying (to run flying animation).
            isGrounded = false; // Always set flag to false for whenever player may be on a safe floor and wants to get up.
            yVelocity = flySpeed * scoreManager.getSpeed(); // move player up. 
        }
        else if (moveDirection.y < 0) // If DOWN pressed,
        {
            anim.enabled = false; // Disable Animator component when player is on safe floor (to use gliding sprite).
            flapTimer = 0f; // Reset flapTimer back to 0 seconds (so we can reuse UP animation loop cleanly). 
            useFirstFrame = true; // Reset flag back to true to reuse UP animation loop cleanly. 
            sr.sprite = glidingSprite; // Change the player sprite (on the SpriteRenderer component) to show gliding frame. 
            yVelocity = -fallSpeed * scoreManager.getSpeed(); // move player down. 
        }
        else // If UP released, 
        {
            anim.enabled = false; // Disable Animator component when player is on safe floor (to use gliding sprite).
            flapTimer = 0f; // Reset flapTimer back to 0 seconds (so we can reuse UP animation loop cleanly). 
            useFirstFrame = true; // Reset flag back to true to reuse UP animation loop cleanly. 
            sr.sprite = glidingSprite; // Change the player sprite to show gliding frame. 
            yVelocity = -fallSpeed * scoreManager.getSpeed(); // again, move player down (same as if DOWN pressed). 
        }

        if (isGrounded) // If player is on a safe floor, 
        {
            anim.enabled = false; // Disable Animator component when player is on safe floor (to use resting sprite). 
            sr.sprite = restingSprite; // Change the player sprite to show resting frame. 
            rb.linearVelocity = Vector2.zero; // stop all movement if player is on a safe floor. 
            return; // Do not continue making a non-vector as we want player to completely stop. 
        }

        rb.linearVelocity = new Vector2(xVelocity, yVelocity); // Create movement non-vector based on assigned x and y velocities. 
    }
}