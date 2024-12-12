using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("p"))
        {
            if(!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit(); 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Debug.Log("PAUSED");
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Debug.Log("UNPAUSED");
    }

    bool pauseToggle()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}
