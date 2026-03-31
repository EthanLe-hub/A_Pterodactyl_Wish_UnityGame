// Ethan Le (3/25/2026):
using UnityEngine; 
using UnityEngine.UI; 
using System.Collections;
using TMPro;

/** 
 * Script to manage player dialogue depending on player current position: 
**/ 
public class PlayerDialogueManager : MonoBehaviour
{
    // Components for player dialogue: 
    public Image background; // Black box background contrast against white text. 
    public Image playerSprite; // Image of player. 
    public TextMeshProUGUI dialogue; // White text for player dialogue. 

    private Coroutine dialCoroutine; // To store the current dialogue time coroutine. 

     // Hide all UI at start
    void Start()
    {
        HideAllUI();
    }

    public void HideAllUI()
    {
        background.enabled = false;
        playerSprite.enabled = false;
        dialogue.enabled = false;
    }

    // Show a specific dialogue line by index
    public void ShowDialogueLine(string line)
    {
        // Check if a previous dialogue is still onscreen:
        if (dialogue.enabled == true)
        {
            // Set all components back to false and reset coroutine before setting new dialogue:
            background.enabled = false; 
            playerSprite.enabled = false; 
            dialogue.enabled = false; 
            
            StopCoroutine(dialCoroutine); // Stop the coroutine. 

            // Wait for a short amount of time before showing new dialogue: 
            HideAfterSeconds(1f);  
        }

        background.enabled = true;
        playerSprite.enabled = true;
        dialogue.enabled = true;
        dialogue.text = line;

        // Hide automatically after a few seconds:
        dialCoroutine = StartCoroutine(HideAfterSeconds(10f)); // Start time duration for dialogue appearance. 
    }

    // Have the player dialogue pop up: 
    private IEnumerator HideAfterSeconds(float seconds)
    {
        // Wait for specified duration before making dialogue disappear:
        yield return new WaitForSeconds(seconds);

        HideAllUI(); 
    }
}