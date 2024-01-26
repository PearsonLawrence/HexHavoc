using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Made By Braden
public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsMenuPanel;
    public GameObject aboutMenuPanel;
    public GameObject canvas;
    

    public Button startButton;
    public Button optionsButton;
    public Button aboutButton;
    public Button backButton;

    public Button AbackButton;

    public Button exitButton; 

    public LobbyUIManager lobbyManager;

    public Button leaveLobby;
    
    void Start()
    {
        SetMenuState(true, false, false, true);

        lobbyManager.disableLobbyOptions();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(ShowOptionsMenu);
        }

        if (aboutButton != null)
        {
            aboutButton.onClick.AddListener(ShowAboutMenu);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToMainMenu);
        }
        if (AbackButton != null)
        {
            AbackButton.onClick.AddListener(BackToMainMenu);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        if (leaveLobby != null)
        {
            leaveLobby.onClick.AddListener(BackToMainMenu);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowOptionsMenu();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowAboutMenu();
        }
    }

    public void StartGame()
    {
        lobbyManager.enableLobbyOptions();
        SetMenuState(false, false, false, false);
        
    }

    public void ShowOptionsMenu()
    {
        SetMenuState(false, true, false, true);
    }

    public void ShowAboutMenu()
    {
        SetMenuState(false, false, true, true );
    }

    public void OpenOptionsSettings()
    {
        SetMenuState(false, true, false,true );
    }

    public void CloseOptionsSettings()
    {
        SetMenuState(false, true, false, true);
    }

    public void BackToMainMenu()
    {
        SetMenuState(true, false, false, true);
        lobbyManager.disableLobbyOptions();

    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    private void SetMenuState(bool mainMenuActive, bool optionsMenuActive, bool aboutMenuActive,bool canvasActive)
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(mainMenuActive);
        }

        if (optionsMenuPanel != null)
        {
            optionsMenuPanel.SetActive(optionsMenuActive);
        }

        if (aboutMenuPanel != null)
        {
            aboutMenuPanel.SetActive(aboutMenuActive);
        }
        if (canvas != null)
        {
            canvas.SetActive(mainMenuActive);
        }
       
    }
}



