// Ethan Le (3/25/2026):
using System;
using System.Collections.Generic; 
using System.Linq; // For using Reverse() to get top Keys from the Sorted Dictionary. 
using System.Text; // For StringBuilder. 
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
    public GameObject titleScreen; // Contains the title screen for the game. 

    // Title Screen components:
    public Button playButton; 
    public Button creditsButton; 
    public Button scoreboardButton; 
    public Button closeCreditsButton; 
    public Button closeScoreboardButton;
    public GameObject creditsPage; 
    public GameObject scoreboardPage; 
    public TextMeshProUGUI scoreText; 

    // Flag to mark narration being complete: 
    public bool introPlayed = false; // Always play intro during every first playthrough. 
    public bool midStoryPlayed = false; // Always play MIDWAY story sequence during every first playthrough. 

    // Flag to make it so that the OnSceneLoaded function does NOT run on first load (it should only run upon player death):
    private bool hasInitialized = false; 

    // Int variable to keep track of player death count (needs to be in GameManager so it persists upon game reloads):
    private int deathCount = 0; 

    // A Sorted Dictionary (Java equivalent of a TreeMap) that keeps records of a player's score and death count (Score, Death Count):
    // Sorts by Key (Score):
    SortedDictionary<int, int> playerData = new SortedDictionary<int, int>(); 

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
        titleScreen = GameObject.Find("TitleScreen"); 
        scoreManager = GameObject.FindObjectOfType<ScoreManager>(); 

        // Re-find title components and buttons and re-attach listeners after game reloads (like after the player dies):
        playButton = GameObject.Find("PlayButton").GetComponent<Button>(); 
        creditsButton = GameObject.Find("CreditsButton").GetComponent<Button>();
        scoreboardButton = GameObject.Find("ScoreboardButton").GetComponent<Button>(); 
        creditsPage = GameObject.Find("CreditsPage"); 
        scoreboardPage = GameObject.Find("ScoreboardPage"); 
        closeCreditsButton = GameObject.Find("CloseCreditsButton").GetComponent<Button>(); 
        closeScoreboardButton = GameObject.Find("CloseScoreboardButton").GetComponent<Button>(); 
        GameObject scoreGameObject = GameObject.Find("ScoreText"); 
        scoreText = scoreGameObject.GetComponent<TextMeshProUGUI>(); 

        SetupTitleButtons(); 
            
        // Restart game flow w/o the intro narration (taken care of by "StartGame()" function): 
        titleScreen.SetActive(false); // Do not return to title screen. 
        // No story sequences, just open up scoreboard and player speed and the level itself: 
        if (storySequence != null)
        {
            storySequence.SetActive(false); 
        }

        if (secStorySequence != null)
        {
            secStorySequence.SetActive(false); 
        }
        StartGame(); // Skips beginning story narration. 
    }

    void Start() // Only runs ONCE the whole game (when you first start the game): 
    {
        // Set flags to false so it is a fresh run: 
        introPlayed = false; 
        midStoryPlayed = false; 

        // The game starts with the story sequence on first playthrough:
        titleScreen.SetActive(true); // Game begins with title screen. 
        creditsPage.SetActive(false); // Ensure Credits page is off when title screen shows up. 
        scoreboardPage.SetActive(false); // Ensure Scoreboard page is off when title screen shows up. 
        storySequence.SetActive(false); 
        secStorySequence.SetActive(false); 
        gameScene.SetActive(false); 
        playerDialogueCanvas.SetActive(true); // Need to stay true to trigger dialogue later. 
        firstGameLevel.SetActive(false); 

        SetupTitleButtons(); // Call function to add listeners to Title Screen Buttons. 
    }

    void SetupTitleButtons()
    {
        // Remove any potential existing listeners to ensure we do not add extra listeners: 
        playButton.onClick.RemoveAllListeners(); 
        creditsButton.onClick.RemoveAllListeners();
        scoreboardButton.onClick.RemoveAllListeners(); 
        closeCreditsButton.onClick.RemoveAllListeners(); 
        closeScoreboardButton.onClick.RemoveAllListeners(); 

        // Attach onClick listeners to Title Screen buttons:
        playButton.onClick.AddListener(() => {
            titleScreen.SetActive(false);
            storySequence.SetActive(true); // When player clicks Play button, commence story sequence. 
        }); 
        creditsButton.onClick.AddListener(() => creditsPage.SetActive(true)); // Show Credits page if Credits button is clicked. 
        scoreboardButton.onClick.AddListener(() => {
            scoreboardPage.SetActive(true); 
            displayHighScores(); 
        }); // Show Scoreboard page with the high scores if Scoreboard button is clicked. 
        closeCreditsButton.onClick.AddListener(() => creditsPage.SetActive(false)); // Close Credits page if Close Credits button is clicked. 
        closeScoreboardButton.onClick.AddListener(() => scoreboardPage.SetActive(false)); // Close Scoreboard page if Close Scoreboard button is clicked. 
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

    /**
     * Function to return to the Title screen (upon quitting mid-game or after completing the game):
    **/ 
    public void returnToTitle()
    {
        // Reset important components and variables for new run:
        deathCount = 0;
        scoreManager.resetScore(); 

        // The game starts with the story sequence on first playthrough:
        titleScreen.SetActive(true); // Game begins with title screen. 
        creditsPage.SetActive(false); // Ensure Credits page is off when title screen shows up. 
        scoreboardPage.SetActive(false); // Ensure Scoreboard page is off when title screen shows up. 
        storySequence.SetActive(false); 
        secStorySequence.SetActive(false); 
        gameScene.SetActive(false); 
        playerDialogueCanvas.SetActive(true); // Need to stay true to trigger dialogue later. 
        firstGameLevel.SetActive(false); 

        // Set flags to false so it is a fresh run: 
        introPlayed = false; 
        midStoryPlayed = false; 
    }

    /**
     * Function to retrieve current player death count: 
    **/
    public int getDeathCount()
    {
        return deathCount; 
    }

    /** 
     * Function to increment player death count (when player dies):
    **/
    public void incrementDeathCount()
    {
        deathCount++; 
    }

    /**
     * Function to get score from Score Manager:
    **/
    public int getNewScore()
    {
        return scoreManager.getScore(); 
    }

    /** 
     * Function to add score and death count to Sorted Dictionary data after player completes the game: 
    **/
    public void addToPlayerData()
    {
        // Write a brand new key-value pair if player's score has not been achieved before: 
        if (!playerData.ContainsKey(getNewScore()))
        {
            playerData.Add(getNewScore(), deathCount); 
        }

        // Otherwise, replace the existing score with the new death count IF the death count is lower than previously (takes the better record):
        else 
        {
            playerData.TryGetValue(getNewScore(), out int oldDeathCount); // Get the old death count from the Key. 

            if (deathCount < oldDeathCount)
            {
                playerData[getNewScore()] = deathCount; // dict[key] = value; 
            }
        }
    }

    /** 
     * Function to display the top 5 Scores and their Death Counts in the Scoreboard Page:
    **/
    public void displayHighScores()
    {
        StringBuilder newString = new StringBuilder(); // To append each of the top 5 scores into for display. 

        int i = 0; // Keeps track of how many pairs we have already added to the display. 

        // Loop through each Key-Value pair in the Sorted Dictionary starting with the last one (the highest): 
        foreach (var pair in playerData.Reverse())
        {
            if (i < 5)
            {
                newString.AppendLine("Score: " + pair.Key + "    |    " + "Death Count: " + pair.Value + "\n"); 
                i++; 
            }

            else // If 5 scores already have been added, break out of the loop. 
            {
                break; 
            }
        }

        if (scoreText != null) // Safety check to ensure we have the TMPro component for displaying the top 5 scores and their death counts.  
        {
            scoreText.text = newString.ToString(); // Set the newly built string to be displayed. 
        }

        Debug.Log("Successful Score Display!"); 
    }
}