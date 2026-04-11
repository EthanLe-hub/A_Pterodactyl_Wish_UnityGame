// Ethan Le (3/21/2026):
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

/** 
 * This script manages the player's score and speed upon collecting a purple coin.
**/ 
public class ScoreManager : MonoBehaviour 
{
    // Variables for UI components to display: 
    private int score = 0; 
    private float speedMultiplier = 1f; 
    
    // UI components to display on Unity Canvas (assign in Unity Inspector): 
    public TextMeshProUGUI playerScoreboard; 
    public TextMeshProUGUI currPlayerSpeed; 
    public TextMeshProUGUI deathCountUI; 

    // Upon starting the game, call function to initialize default score and speed.  
    void Start()
    {
        CreateUI(); 
    }

    // Upon calling this function after narration, ensure we get the text components to display on the Canvas:
    public void CreateUI()
    {
        playerScoreboard.text = "Score: " + score; // Display the default score of 0. 
        
        currPlayerSpeed.text = "Current Speed: " + speedMultiplier; // Display the default speed of 1x. 

        deathCountUI.text = "Death Count: " + GameManager.instance.getDeathCount(); // Display the default death count of 0 (from GameManager singleton instance). 
    }

    public void collectCoin()
    {
        // Update the backend score and speed multiplier:
        score += 10; 
        speedMultiplier += 0.10f; 

        playerScoreboard.text = "Score: " + score; // Update the displayed score. 
        
        currPlayerSpeed.text = "Current Speed: " + speedMultiplier; // Update the displayed speed. 
    }

    public void collectSlowCoin()
    {
        // Update the backend score and speed multiplier: 
        
        score += 10; 

        /*if (score < 0)
        {
            score = 0; 
        }*/

        speedMultiplier -= 0.10f; 

        playerScoreboard.text = "Score: " + score; // Update the displayed score. 

        currPlayerSpeed.text = "Current Speed: " + speedMultiplier; // Update the displayed speed. 
    }

    public void collectBigGoldCoin()
    {
        // Update the backend score only (gold coins are simply rewards): 
        score += 500; // Add 500 points for big gold coins. 

        playerScoreboard.text = "Score : " + score; // Update the displayed score. 
    }

    public void collectMedGoldCoin()
    {
        // Update the backend score only (gold coins are simply rewards): 
        score += 250; // Add 250 points for medium gold coins. 

        playerScoreboard.text = "Score : " + score; // Update the displayed score. 
    }

    public void collectSmallGoldCoin()
    {
        // Update the backend score only (gold coins are simply rewards): 
        score += 10; // Add 10 points for small gold coins. 

        playerScoreboard.text = "Score : " + score; // Update the displayed score. 
    }

    public void incDeathCount()
    {
        // Update the player's death count whenever scene reloads (AKA, when the player dies):
        GameManager.instance.incrementDeathCount();  

        deathCountUI.text = "Death Count: " + GameManager.instance.getDeathCount(); // Update the displayed death count. 
    }

    // Function to retrieve the speed multiplier to update player speed upon collecting a purple coin: 
    public float getSpeed()
    {
        return speedMultiplier; 
    }

    // Function to retrieve score of the player (for when the player finishes the game):
    public int getScore()
    {
        return score; 
    }

    // Function to reset score after game completion: 
    public void resetScore()
    {
        score = 0;
    }
}