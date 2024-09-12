using System.Collections;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject display_mainMenu;
    [SerializeField] GameObject display_leaderboard;
    [SerializeField] GameObject display_accountCreateSelect;
    [SerializeField] GameObject display_gamemodeSelect;
    [SerializeField] GameObject display_trashPicking_tutorial;
    [SerializeField] GameObject display_trashPicking_levelSelect;
    [SerializeField] GameObject display_speciesIdentification_tutorial;
    [SerializeField] GameObject display_speciesIdentification_levelSelect;

    public static MainMenuHandler Instance;
    public bool SceneChecked { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        string resetSelection = PlayerPrefs.GetString("SelectedLevel");
        if (resetSelection != null)
        {
            PlayerPrefs.SetString("SelectedLevel", string.Empty);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        // FindObjects();
    }

    private void Update()
    {
        if (GameSceneManager.Instance.CurrentScene() == "_Menu" && !SceneChecked)
        {
            SceneChecked = true;
            
            if(PlayerPrefs.GetString("gameFinished") != string.Empty)
            {
                StartCoroutine(WaitToLoad());
                PlayerPrefs.SetString("gameFinished", string.Empty);
            }

            Debug.Log($"⚠️ CURRENT SCENE IS MENU!");
        }
        else if (GameSceneManager.Instance.CurrentScene() != "_Menu")
            SceneChecked = false;
    }

    // void FindObjects()
    // {
    //     display_mainMenu = GameObject.Find("Main Menu Display");
    //     display_leaderboard = GameObject.Find("Leaderboard Display");
    //     display_accountCreateSelect = GameObject.Find("Account Create Select Display");
    //     display_gamemodeSelect = GameObject.Find("Game Mode Select Display");
    //     display_trashPicking_tutorial = GameObject.Find("Trash Picking Tutorial Display");
    //     display_trashPicking_levelSelect = GameObject.Find("Trash Picking Level Selection Display");
    //     display_speciesIdentification_tutorial = GameObject.Find("Species Identification Tutorial Display");
    //     display_speciesIdentification_levelSelect = GameObject.Find("Species Identification Level Selection Display");
    //     Debug.Log($"FOUND MENU DISPLAYS!");
    // }

    public void Button_leaderboard()
    {
        FlipState(display_mainMenu);                //disable
        FlipState(display_leaderboard);             //enable
    }

    public void Button_AccountSelectCreate()
    {
        FlipState(display_mainMenu);                //disable
        FlipState(display_accountCreateSelect);     //enable
    }

    public void Button_GamemodeSelect()
    {
        FlipState(display_accountCreateSelect);     //disable
        FlipState(display_gamemodeSelect);          // enable
    }

    // TRASH PICKING GAME MOODE
    public void Button_TP_tutorial()
    {
        FlipState(display_gamemodeSelect);          //disable
        FlipState(display_trashPicking_tutorial);   //enable
    }

    public void Button_TP_LevelSelect()
    {
        FlipState(display_trashPicking_tutorial);   //disable
        FlipState(display_trashPicking_levelSelect);//enable
    }

    // SPECIES IDENTIFACTION GAME MODE
    public void Button_SI_tutorial()
    {
        FlipState(display_gamemodeSelect);                      //disable
        FlipState(display_speciesIdentification_tutorial);      //enable
    }

    public void Button_SI_LevelSelect()
    {
        FlipState(display_speciesIdentification_tutorial);      //disable
        FlipState(display_speciesIdentification_levelSelect);   //enable
    }

    public void ReturnMainMenu()
    {
        display_mainMenu.SetActive(true);
        display_leaderboard.SetActive(false);
        display_accountCreateSelect.SetActive(false);
        display_gamemodeSelect.SetActive(false);
        display_trashPicking_tutorial.SetActive(false);
        display_trashPicking_levelSelect.SetActive(false);
        display_speciesIdentification_tutorial.SetActive(false);
        display_speciesIdentification_levelSelect.SetActive(false);
    }

    public IEnumerator WaitToLoad()
    {
        Debug.Log($"⚠️Trying to load~");
        yield return new WaitForSeconds(.3f);
        ReturnMainMenu();
        FlipState(display_accountCreateSelect);   //enable
        FlipState(display_gamemodeSelect);      //enable

        Debug.Log($"CURRENT PLAYER! {PlayerPrefs.GetString("currentPlayer")}");
        ProfilesHandler.Instance.Button_SelectThisProfile(PlayerPrefs.GetString("currentPlayer"));
        
        Debug.Log($"⚠️Loaded!");
    }

    public void ReturnToGamemodeSelect()
    {
        // FindObjects();
        StartCoroutine(WaitToLoad());
        Debug.Log($"ALL GOODS!");
    }

    public void FlipState(GameObject gameObject) => gameObject.SetActive(!gameObject.activeSelf);
}
