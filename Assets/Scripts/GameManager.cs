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
    public GameObject secStorySequence; // Contains the text for the MIDWAY story sequence. 
    public GameObject playerDialogueCanvas; // Contains the player dialogue (for tutorial and narration purposes). 
    public GameObject firstGameLevel; // Contains the level layout and player sprite. 

    // Flag to mark narration being complete: 
    public bool introPlayed = false; // Always play intro during every first playthrough. 
    public bool midStoryPlayed = false; // Always play MIDWAY story sequence during every first playthrough. 

    // Flag to make it so that the OnSceneLoaded function does NOT run on first load (it should only run upon player death):
    private bool hasInitialized = false; 

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
     * Function to properly assign camera to correct level version of the player. 
    **/ 
    void AssignCameraToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // With current level active, retrieve the Player component from it (the other level version will be avoided since it is now off). 
        
        if (player != null)
        {
            Camera.main.GetComponent<CameraFollow>().player = player.transform; // Set the camera to now focus on the current level version of the player. 
        }
    }

    /**
     * Function to ensure only one level is active at one time:
    **/
    void SetActiveLevel()
    {
        firstGameLevel.SetActive(true); // Set current level as active. 
        AssignCameraToPlayer(); // Call function to set camera to the correct player GameObject.
    }

    /**
     * Whenever the scene reloads upon player death, get the scene objects by name and reassign them to access them:
    **/ 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!hasInitialized)
        {
            hasInitialized = true; // Now game will use this function when player dies.
            return; // To skip the first load. 
        }

        // Re-find scene objects after game reloads (like after the player dies): 
        
        gameScene = GameObject.Find("GameScene");
        storySequence = GameObject.Find("StorySequence"); 
        secStorySequence = GameObject.Find("2ndStorySequence");
        playerDialogueCanvas = GameObject.Find("PlayerDialogueCanvas");
        firstGameLevel = GameObject.Find("FirstLevel");
            
        // Restart game flow w/o the intro narration: 
        if (introPlayed == true)
        {
            StartGame(); // Skips beginning story narration. 
        } 
    }

    void Start() // Only runs ONCE the whole game (when you first start the game): 
    {
        // Set flags to false so it is a fresh run: 
        introPlayed = false; 
        midStoryPlayed = false; 

        // The game starts with the story sequence on first playthrough:
        storySequence.SetActive(true); // Game begins with story narration when player first plays. 
        secStorySequence.SetActive(false); 
        gameScene.SetActive(false); 
        playerDialogueCanvas.SetActive(true); // Need to stay true to trigger dialogue later. 
        firstGameLevel.SetActive(false); 
    }

    /**
     * Function to start up the game after story sequence concludes or if story sequence already played:
    **/
    public void StartGame()
    {
        // Story sequence closes, open up scoreboard and player speed and the level itself: 
        if (storySequence != null)
        {
            storySequence.SetActive(false); 
        }

        if (secStorySequence != null)
        {
            secStorySequence.SetActive(false); 
        }
        
        gameScene.SetActive(true); 
        SetActiveLevel(); // First level shows. 

        // Score manager already exists in the Canvas itself, no need to reinitialize its texts: 
        //scoreManager.CreateUI(); // Initialize text for score and player speed. 

        // Hide the actual player dialogue visuals until player touches dialogue trigger zone:
        PlayerDialogueManager dm = playerDialogueCanvas.GetComponent<PlayerDialogueManager>();
        if(dm != null)
        {
            dm.HideAllUI(); 
        }
    }

    /**
     * Function to start up the SECOND level after the MIDWAY story sequence concludes or if MIDWAY story sequence already played: 
    **/
    public void ContGame() 
    {
        // MIDWAY story sequence closes, open up scoreboard and player speed and the level itself: 
        if (storySequence != null)
        {
            storySequence.SetActive(false); 
        }

        if (secStorySequence != null)
        {
            secStorySequence.SetActive(false); 
        }
        
        gameScene.SetActive(true); 
        SetActiveLevel(); // Second level now shows (same as the first but with a twist). 

        // Score manager already exists in the Canvas itself, no need to reinitialize its texts: 
        //scoreManager.CreateUI(); // Initialize text for score and player speed. 

        // Hide the actual player dialogue visuals until player touches dialogue trigger zone:
        PlayerDialogueManager dm = playerDialogueCanvas.GetComponent<PlayerDialogueManager>();
        if(dm != null)
        {
            dm.HideAllUI(); 
        }
    }
}