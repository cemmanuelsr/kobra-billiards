using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject HelpMenuUI;

    public void loadGameScene() {
        SceneManager.LoadScene("Game");
    }

    public void callMainMenu() {
        MainMenuUI.SetActive(true);
        HelpMenuUI.SetActive(false);
    }

    public void callHelpMenu() {
        HelpMenuUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }
}
