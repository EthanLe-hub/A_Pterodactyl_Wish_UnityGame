// Ethan Le (3/25/2026):
using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.UI; 
using TMPro;

/** 
 * Script to control INTRO story sequence:
**/ 
public class StoryController : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public Button skipButton; // For skipping intro if player does not want to read. 

    [TextArea(3, 5)]
    public string[] storyLines; // Array -- Fill in Unity Inspector with the beginning narration.

    private int currentIndex = 0;

    void Start()
    {
        if (skipButton != null) // Ensure the Button component is assigned in Unity Inspector. 
        {
            skipButton.onClick.AddListener(SkipIntro); // Add listener to skip intro when button is pressed. 
        }

        ShowCurrentLine(); // Game starts at index 0 in the sequence of narration lines. 
    }

    void Update()
    {
        // Detect ANY key or mouse click:
        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            NextLine(); // Move onto the next line of narration. 
        }
    }

    void ShowCurrentLine()
    {
        storyText.text = storyLines[currentIndex]; // Show current narration line. 
    }

    void NextLine()
    {
        currentIndex++; // Increment the index so we can move on to the next narration line in the sequence. 

        if (currentIndex < storyLines.Length) // Show next line in narration if not at the end. 
        {
            ShowCurrentLine();
        }
        else
        {
            // Otherwise, start the game if the narration is done:  
            GameManager.instance.introPlayed = true; // Set flag to true so intro narration does not play again upon dying. 
            GameManager.instance.StartGame(); // Controlled by singleton GameManager instance. 
        }
    }

    void SkipIntro()
    {
        GameManager.instance.introPlayed = true; // Set flag to true so intro narration does not play again upon dying. 
        GameManager.instance.StartGame(); // Controlled by singleton GameManager instance. 
    }
}