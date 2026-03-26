// Ethan Le (3/25/2026):
using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro;

public class StoryController : MonoBehaviour
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
            // Otherwise, start the game if the narration is done:  
            GameManager.instance.StartGame(); // Controlled by singleton GameManager instance. 
        }
    }
}