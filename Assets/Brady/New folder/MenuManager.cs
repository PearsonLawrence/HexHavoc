using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsMenuPanel;
    public GameObject aboutMenuPanel;
    

    public Button startButton;
    public Button optionsButton;
    public Button aboutButton;
    public Button backButton;

    public Button AbackButton;

    public Button exitButton; 


    void Start()
    {
        SetMenuState(true, false, false);

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
        SceneManager.LoadScene("YourMainGameScene");
    }

    public void ShowOptionsMenu()
    {
        SetMenuState(false, true, false);
    }

    public void ShowAboutMenu()
    {
        SetMenuState(false, false, true );
    }

    public void OpenOptionsSettings()
    {
        SetMenuState(false, false, false );
    }

    public void CloseOptionsSettings()
    {
        SetMenuState(false, true, false);
    }

    public void BackToMainMenu()
    {
        SetMenuState(true, false, false);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    private void SetMenuState(bool mainMenuActive, bool optionsMenuActive, bool aboutMenuActive)
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

       
    }
}



