// Ethan Le (4/4/2026):
using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro;

/** 
 * Script to control MIDWAY story sequence (after completing first level, before playing second level):
**/ 
public class SecondStoryController : MonoBehaviour
{
    public TextMeshProUGUI storyText;

    [TextArea(3, 5)]
    public string[] storyLines; // Array -- Fill in Unity Inspector with the beginning narration.

    private int currentIndex = 0;

    void Start()
    {
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
            GameManager.instance.midStoryPlayed = true; // Set flag to true so midway story sequence does not play again upon dying. 
            /*
            // Otherwise, continue the game (second level) if the narration is done:  
            GameManager.instance.ContGame(); // Controlled by singleton GameManager instance. 
            */

            storyText.text = "Your final score: " + GameManager.instance.getNewScore() + "\nYour death count: " + GameManager.instance.getDeathCount(); 
        }
    }
}