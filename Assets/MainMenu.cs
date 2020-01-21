using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private GameObject _levelPickingMenu;

    
    public void Quit() {
        Application.Quit();
    }

    public void StartGame() {
        SceneManager.LoadScene("1");
    }

    public void ChooseLevel() {
        _levelPickingMenu.SetActive(true);
    }

    public void CloseLevelPicker() {
        _levelPickingMenu.SetActive(false);
    }

    public void PickALevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }
}
