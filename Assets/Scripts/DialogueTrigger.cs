// Ethan Le (3/25/2026):
using UnityEngine;

/**
 * Script to attach to each trigger zone (a GameObject with a Collider2D component) that triggers player dialogue.
**/
public class DialogueTrigger : MonoBehaviour
{
    [TextArea(1, 3)]
    public string dialogueLine; // Fill this specific trigger zone's text with the appropriate player dialogue. 

    private bool triggered = false; // Player has not yet triggered any dialogue trigger zone. 

    void OnTriggerEnter2D(Collider2D other) // Called when the player enters zone of dialogue trigger. 
    {
        if (triggered) return; // Ensure that it is the player that enters it, not any other moving GameObjects. 

        if (other.CompareTag("Player")) // If specifically the player enters the dialogue trigger zone, we show dialogue. 
        {
            triggered = true; // Dialogues are only triggered once per playthrough (attached to each trigger zone that each has different dialogue text). 

            PlayerDialogueManager dialogueManager = Object.FindAnyObjectByType<PlayerDialogueManager>(); // Get the GameObject (containing player image, text, and black bg components) which has the PlayerDialogueManager.cs script. 
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogueLine(dialogueLine); // Show appropriate player dialogue. 
            }
        }
    }
}