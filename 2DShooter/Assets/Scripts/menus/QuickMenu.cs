using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickMenu : MonoBehaviour
{
    // TODO: freeze game when quick menu is open

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
