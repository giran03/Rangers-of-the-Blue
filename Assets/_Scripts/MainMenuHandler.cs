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

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        string resetSelection = PlayerPrefs.GetString("SelectedLevel");
        if (resetSelection != null)
        {
            PlayerPrefs.SetString("SelectedLevel", "");
            PlayerPrefs.Save();
        }
    }
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

    public void FlipState(GameObject gameObject) => gameObject.SetActive(!gameObject.activeSelf);
}
