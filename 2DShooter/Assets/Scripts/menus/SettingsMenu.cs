using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject[] subMenus;

    public void OpenSubMenu(int index)
    {
        for (int i = 0; i < subMenus.Length; i++)
        {
            subMenus[i].SetActive(i == index);
        }
    }
}

