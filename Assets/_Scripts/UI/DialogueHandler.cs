using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string text;
}

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Button continueButton;

    [Tooltip("Whether to check if stage_1_cleared is true before displaying dialogue")]
    [SerializeField] bool checkStageCleared = true;

    [SerializeField] Dialogue[] dialogues;

    private int currentDialogueIndex = 0;

    void Start()
    {
        continueButton.onClick.AddListener(NextDialogue);
        if (!CheckPlayerLevel())
        {
            StartDialogue();
        }
        else
            DestroyDialogue();
    }

    public void StartDialogue()
    {
        DisplayDialogue();
        continueButton.gameObject.SetActive(true);
    }

    public void DisplayDialogue()
    {
        GameSceneManager.Instance.PauseGame();

        Dialogue dialogue = dialogues[currentDialogueIndex];
        dialogueText.text = dialogue.text;
    }

    public void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
            DisplayDialogue();
        else
            DestroyDialogue();
    }

    void DestroyDialogue()
    {
        Debug.Log("End of dialogue");
        Destroy(gameObject);
        GameSceneManager.Instance.ResumeGame();
    }

    bool CheckPlayerLevel()
    {
        if (!checkStageCleared)
            return false;

        PlayerData data = SaveSystem.LoadPlayer(SaveSystem.SelectedProfileName);
        return data.stage_1_cleared;
    }
}
