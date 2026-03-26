// Ethan Le (3/25/2026):
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

/**
 * Script to control game scenes.
**/ 
public class GameManager : MonoBehaviour
{
    // Game scenes:
    public GameObject gameScene; // Contains the scoreboard and player speed. 
    public GameObject storySequence; // Contains the text for the beginning of the story. 
    public GameObject playerDialogueCanvas; // Contains the player dialogue (for tutorial and narration purposes). 
    public GameObject gameLevel; // Contains the level layout and player sprite. 

    // Singleton instance of GameManager: 
    public static GameManager instance; 

    // Assign the ScoreManager directly via Unity Inspector:
    public ScoreManager scoreManager; 

    void Awake() // Upon game loading, create a singleton instance of the game if it does not already exist:
    {
        if (instance == null) // If GameManager singleton instance does not yet exist,
        {
            instance = this; // then assign this instance as the static GameManager singleton instance. 
        }

        else
        {
            Destroy(gameObject); // Prevent clones of the singleton instance.  
        }
    }

    void Start()
    {
        // The game starts with the story sequence: 
        storySequence.SetActive(true);  
        gameScene.SetActive(false); 
        playerDialogueCanvas.SetActive(true); // Need to stay true to trigger dialogue later. 
        gameLevel.SetActive(false); 
    }

    /**
     * Function to start up the game after story sequence concludes:
    **/
    public void StartGame()
    {
        // Story sequence closes, open up scoreboard and player speed and the level itself: 
        storySequence.SetActive(false); 
        gameScene.SetActive(true); 
        gameLevel.SetActive(true);

        scoreManager.CreateUI(); // Initialize text for score and player speed. 

        // Hide the actual visuals
        PlayerDialogueManager dm = playerDialogueCanvas.GetComponent<PlayerDialogueManager>();
        if(dm != null)
        {
            dm.HideAllUI(); 
        }
    }

    /**
     * Function to have player dialogue show up:
    **/
    public void EnablePlayerDiag()
    { 
        playerDialogueCanvas.SetActive(true); 
    }

    /**
     * Function to have player dialogue disappear:
    **/
    public void DisablePlayerDiag()
    { 
        playerDialogueCanvas.SetActive(false); 
    }

}