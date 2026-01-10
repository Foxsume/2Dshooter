using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickMenu : MonoBehaviour
{
    public void Resume()
    {
        // Unfreeze the game
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
