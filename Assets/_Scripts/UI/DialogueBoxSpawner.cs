using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject dialogueBoxPrefab;
    [SerializeField] Transform parent;
    [SerializeField] Vector3 spawn_offset;

    public void OnButtonClick()
    {
        // Calculate screen center
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 centerPosition = new(screenWidth / 2, screenHeight / 2, 0);

        // Instantiate the dialogue box
        GameObject dialogueBox = Instantiate(dialogueBoxPrefab, parent);

        // Set the dialogue box's position
        dialogueBox.transform.position = centerPosition + spawn_offset;
    }
}
