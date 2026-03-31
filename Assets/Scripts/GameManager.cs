// Ethan Le (3/25/2026):
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // To control scene reloads. 
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

    // Flag to mark narration being complete: 
    private bool introPlayed = false; // Always play intro during every first playthrough. 

    // Singleton instance of GameManager: 
    public static GameManager instance; 

    // Assign the ScoreManager directly via Unity Inspector:
    public ScoreManager scoreManager; 

    void Awake() // Upon game loading, create a singleton instance of the game if it does not already exist:
    {
        if (instance == null) // If GameManager singleton instance does not yet exist,
        {
            instance = this; // then assign this instance as the static GameManager singleton instance. 
            DontDestroyOnLoad(gameObject); // GameManager singleton instance survives game reload (upon player dying). 
        }

        else
        {
            Destroy(gameObject); // Prevent clones of the singleton instance.  
        }
    }

    /**
     * Subscribe to the function that retrieves the scene objects again after game reloads (from player dying): 
    **/ 
    void OnEnable() // Runs upon starting the game for the first time (for potential game reloads upon player death). 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /**
     * Unsubscribe to the function that retrieves the scene objects again after game reloads (from player dying):
    **/ 
    void OnDisable() // Runs upon stopping the game (for cleanup). 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /**
     * Whenever the scene reloads upon player death, get the scene objects by name and reassign them to access them:
    **/ 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-find scene objects after game reloads (like after the player dies): 
        gameScene = GameObject.Find("GameScene");
        storySequence = GameObject.Find("StorySequence");
        playerDialogueCanvas = GameObject.Find("PlayerDialogueCanvas");
        gameLevel = GameObject.Find("TutorialLevel");

        // Restart game flow w/o the intro narration: 
        if (introPlayed == true)
        {
            StartGame(); // Skips beginning story narration. 
        }
    }

    void Start() // Only runs ONCE the whole game (when you first start the game): 
    {
        // The game starts with the story sequence on first playthrough:
        storySequence.SetActive(true); // Game begins with story narration when player first plays. 
        gameScene.SetActive(false); 
        playerDialogueCanvas.SetActive(true); // Need to stay true to trigger dialogue later. 
        gameLevel.SetActive(false); 

        introPlayed = true; // Set flag to true so intro narration does not play again upon dying. 
    }

    /**
     * Function to start up the game after story sequence concludes or if story sequence already played:
    **/
    public void StartGame()
    {
        // Story sequence closes, open up scoreboard and player speed and the level itself: 
        storySequence.SetActive(false); 
        gameScene.SetActive(true); 
        gameLevel.SetActive(true);

        // Score manager already exists in the Canvas itself, no need to reinitialize its texts: 
        //scoreManager.CreateUI(); // Initialize text for score and player speed. 

        // Hide the actual visuals:
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