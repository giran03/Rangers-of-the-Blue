using System;
using TMPro;
using UnityEngine;

[Serializable]
public class Trash
{
    public string trash_name;
    public string trash_information;
}

public class TrashScanHandler : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] Trash[] trashes;
    [SerializeField] GameObject information_popup_window;

    [SerializeField] GameObject button_information;
    [SerializeField] TMP_Text label_trash_name;
    [SerializeField] TMP_Text label_trash_info;

    Trash current_trash;
    bool isScanning;

    private void Start()
    {
        button_information.SetActive(false);
        information_popup_window.SetActive(false);
    }

    private void Update()
    {
        Ray cameraRay = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        if (Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.tag.Contains("Trash"))
            {
                button_information.SetActive(true);

                Debug.Log($"SCANNING {hit.collider.name} !");

                GetTrashInformation(hit.collider.name);
            }
        }
        else
        {
            button_information.SetActive(false);
            information_popup_window.SetActive(false);

            if (isScanning)
            {
                isScanning = false;
                GameSceneManager.Instance.ResumeGame();
            }
        }


    }


    void GetTrashInformation(string trashName)
    {
        string cleanedTrashName = trashName.Replace("(Clone)", "");
        current_trash = Array.Find(trashes, x => x.trash_name.Contains(cleanedTrashName));
        if (current_trash != null)
        {
            Debug.Log($"Settings trash information to window~");
            label_trash_name.SetText($"{current_trash.trash_name}");
            label_trash_info.SetText($"{current_trash.trash_information}");
        }
        else
            Debug.Log("Cant find trash data");
    }

    // Used by 'Information Button'
    public void Button_OpenScanWindow()
    {
        isScanning = true;

        information_popup_window.SetActive(true);
        GameSceneManager.Instance.PauseGame();

        // 🔊 SFX
        AudioManager.Instance.PlaySFX("Information Ping");
    }
}
