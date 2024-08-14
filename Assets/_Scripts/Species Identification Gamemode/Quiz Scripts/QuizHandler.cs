using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuizHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> buttonsCollection;
    public string quizAnswer;

    public QuizType quizType;

    public enum QuizType
    {
        Name,
        ScientificName,
        Conservation
    }

    private void Start()
    {
        Debug.Log($"STARTING QUIZ NAME!");
        // set the quiz answer

        if (QuizType.Name == quizType)
            quizAnswer = SI_Manager.Instance.currentSpecies.speciesName;
        else if (QuizType.ScientificName == quizType)
            quizAnswer = SI_Manager.Instance.currentSpecies.scientificName;
        else if (QuizType.Conservation == quizType)
            quizAnswer = SI_Manager.Instance.currentSpecies.conservationStatus;

        AddListeners();

        Debug.Log($"Currently scanning: {SI_Manager.Instance.currentSpecies.speciesName}");
    }



    void AddListeners()
    {
        int correctButton = UnityEngine.Random.Range(1, 4); // 1-3

        foreach (GameObject buttons in buttonsCollection)
        {
            Debug.Log($"Correct button number: {correctButton}");

            EventTrigger trigger = buttons.transform.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new() { eventID = EventTriggerType.PointerDown };

            if (buttons.name.Contains(correctButton.ToString())) // Adds correct answer listener
            {
                Debug.Log($"Added correct answer to {buttons.name}");
                entry.callback.AddListener((data) => Button_QuizAnswer((PointerEventData)data, quizAnswer));

                //renames the button
                buttons.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = quizAnswer;
            }
            else
            {
                // adds events to the rest of buttons
                AddFillerButtons(entry, buttons, ButtonTextGenerator());
            }
            trigger.triggers.Add(entry);
        }
    }

    void AddFillerButtons(EventTrigger.Entry entry, GameObject buttons, string[] buttonText)
    {
        int randomIndex = UnityEngine.Random.Range(0, buttonText.Length);

        entry.callback.AddListener((data) => Button_QuizAnswer((PointerEventData)data, buttonText[randomIndex]));
        //renames the button
        buttons.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = buttonText[randomIndex];
    }

    string[] ButtonTextGenerator()
    {
        if (QuizType.Name == quizType)
        {
            // creates array from species name;
            string[] buttonNames = SI_Manager.Instance._species.Select(species => species.speciesName).ToArray();
            //remove the name of the correct species
            return buttonNames = buttonNames.Where(x => x != SI_Manager.Instance.currentSpecies.speciesName).ToArray();
        }
        else if (QuizType.ScientificName == quizType)
        {
            // creates array from species scientificName;
            string[] buttonNames = SI_Manager.Instance._species.Select(species => species.scientificName).ToArray();
            //remove the scientificName of the correct species
            return buttonNames = buttonNames.Where(x => x != SI_Manager.Instance.currentSpecies.scientificName).ToArray();
        }
        else if (QuizType.Conservation == quizType)
        {
            // creates array from species conservationStatus;
            string[] buttonNames = { "Least Concern", "Endangered", "Critically Endangered" };
            //remove the conservationStatus of the correct species
            return buttonNames = buttonNames.Where(x => x != SI_Manager.Instance.currentSpecies.conservationStatus).ToArray();
        }
        else return null;
    }

    /// <summary>
    /// Gets the answer from the quiz questionnaires buttons
    /// </summary>
    public void Button_QuizAnswer(PointerEventData data, string answer)
    {
        Debug.Log($"CLICKED! quiz button answer of {answer}");

        if (QuizType.Name == quizType)
        {
            if (answer == SI_Manager.Instance.currentSpecies.speciesName)
            {
                Debug.Log($"ANSWER IS CORRECT!\nSPECIES NAME IS {SI_Manager.Instance.currentSpecies.speciesName}");

                SI_Manager.Instance.SpeciesScoring(SI_Manager.Instance.currentSpecies.speciesName, true);

                SI_Manager.Instance.DestroyQuiz();
                GameSceneManager.Instance.ResumeGame();
            }
            else
            {
                Debug.Log($"INCORRECT ANSWER!");
                SI_Manager.Instance.SpeciesScoring(SI_Manager.Instance.currentSpecies.speciesName, false);

                SI_Manager.Instance.DestroyQuiz();
                GameSceneManager.Instance.ResumeGame();
            }
        }
        else if (QuizType.ScientificName == quizType)
        {
            if (answer == SI_Manager.Instance.currentSpecies.scientificName)
            {
                Debug.Log($"ANSWER IS CORRECT");

                SI_Manager.Instance.SpeciesScoring(SI_Manager.Instance.currentSpecies.speciesName, true);

                SI_Manager.Instance.DestroyQuiz();
                GameSceneManager.Instance.ResumeGame();
            }
            else
            {
                Debug.Log($"INCORRECT ANSWER!");
                SI_Manager.Instance.SpeciesScoring(SI_Manager.Instance.currentSpecies.speciesName, false);

                SI_Manager.Instance.DestroyQuiz();
                GameSceneManager.Instance.ResumeGame();
            }
        }
        else if (QuizType.Conservation == quizType)
        {
            if (answer == SI_Manager.Instance.currentSpecies.conservationStatus)
            {
                Debug.Log($"ANSWER IS CORRECT");

                SI_Manager.Instance.SpeciesScoring(SI_Manager.Instance.currentSpecies.speciesName, true);

                SI_Manager.Instance.DestroyQuiz();
                GameSceneManager.Instance.ResumeGame();
            }
            else
            {
                Debug.Log($"INCORRECT ANSWER!");
                SI_Manager.Instance.SpeciesScoring(SI_Manager.Instance.currentSpecies.speciesName, false);

                SI_Manager.Instance.DestroyQuiz();
                GameSceneManager.Instance.ResumeGame();
            }
        }

    }

    private void OnDestroy()
    {
        Debug.Log($"⚠️ Reseting Current Species to NULL !");
        SI_Manager.Instance.currentSpecies = null;
    }
}
