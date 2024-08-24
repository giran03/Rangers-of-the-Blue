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
    [SerializeField] GameObject nextDialogue = null;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Button continueButton;

    [Tooltip("Check if stage_1_cleared in player save file is true to disable or enable dialogue")]
    [SerializeField] bool checkStageCleared = true;
    [SerializeField] bool is_TP_Level = true;

    [Tooltip("Enable if this dialogue is instantiated in runtime; else the dialogue will be disabled after use and can be re-enabled")]
    [SerializeField] bool isSpawnable = true;

    [SerializeField] AudioClip[] dialogueSFX;

    [SerializeField] Dialogue[] dialogues;

    bool isPaused;
    int currentDialogueIndex = 0;

    void Start()
    {
        SetButton();
    }

    public void SetButton()
    {
        continueButton.onClick.RemoveListener(NextDialogue);
        continueButton.onClick.AddListener(NextDialogue);
        currentDialogueIndex = 0;

        if (!CheckPlayerLevel())
            StartDialogue();
        else
            DisableDialogue();
    }

    public void StartDialogue()
    {
        DisplayDialogue();
        continueButton.gameObject.SetActive(true);
    }

    public void DisplayDialogue()
    {
        AudioManager.Instance.StopSFX();

        if (!isPaused)
        {
            GameSceneManager.Instance.PauseGame();
            isPaused = true;
        }

        Dialogue dialogue = dialogues[currentDialogueIndex];
        dialogueText.text = dialogue.text;

        // play voice over sfx
        if (dialogueSFX != null)
            AudioManager.Instance.PlayDialogue(dialogueSFX, currentDialogueIndex);
    }

    public void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
            DisplayDialogue();
        else
            DisableDialogue();
    }

    void DisableDialogue()
    {
        Debug.Log("End of dialogue");
        if (isSpawnable)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);

        if (nextDialogue != null)
            nextDialogue.SetActive(true);

        isPaused = false;
        GameSceneManager.Instance.ResumeGame();
    }

    bool CheckPlayerLevel()
    {
        if (!checkStageCleared)
        {
            return false;
        }
        else
        {
            PlayerData data = SaveSystem.LoadPlayer(SaveSystem.SelectedProfileName);

            if (is_TP_Level)
                return data.stage_1_cleared;
            else
                return data.stage_SI_1_cleared;
        }
    }
}
