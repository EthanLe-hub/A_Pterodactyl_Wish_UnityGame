// Ethan Le (3/21/2026):
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

/** 
 * This script manages the player's score and speed upon collecting a purple coin.
**/ 
public class ScoreManager : MonoBehaviour 
{
    private int score = 0; 
    private float speedMultiplier = 1f; 

    // UI components to display on Unity Canvas: 
    private TextMeshProUGUI playerScoreboard; 
    private TextMeshProUGUI currPlayerSpeed; 

    // Upon loading the game, ensure we get the text components to display on the Canvas: 
    void Start()
    {
        playerScoreboard = transform.Find("Scoreboard")?.GetComponent<TextMeshProUGUI>(); 
        if (playerScoreboard == null)
        {
            Debug.Log("Scoreboard TMPro component not found!"); 
        }

        currPlayerSpeed = transform.Find("Speed")?.GetComponent<TextMeshProUGUI>(); 
        if (currPlayerSpeed == null)
        {
            Debug.Log("Speed TMPro component not found!"); 
        }

        playerScoreboard.text = "Score: " + score; // Display the default score of 0. 
        
        currPlayerSpeed.text = "Current Speed: " + speedMultiplier; // Display the default speed of 1x. 
    }

    public void collectCoin()
    {
        // Update the backend score and speed multiplier:
        score += 50; 
        speedMultiplier += 0.10f; 

        playerScoreboard.text = "Score: " + score; // Update the displayed score. 
        
        currPlayerSpeed.text = "Current Speed: " + speedMultiplier; // Update the displayed speed. 
    }

    // Function to retrieve the speed multiplier to update player speed upon collecting a purple coin: 
    public float getSpeed()
    {
        return speedMultiplier; 
    }
}