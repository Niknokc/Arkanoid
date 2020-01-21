using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResetter : MonoBehaviour {
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Text youWonText;
    private void Start() {
        if (SceneManager.GetActiveScene().name == "3" && continueButton!=null) {
            continueButton.gameObject.SetActive(false);
            youWonText.gameObject.SetActive(true);
        }
        scoreText.text = gameMgr.instance().GetScore().ToString();
    }

    public void ResetGame() {
        SceneManager.LoadScene("1");
    }

    public void ToMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void ContinueGame() {
        SceneManager.LoadScene((Int32.Parse(SceneManager.GetActiveScene().name)+1).ToString());
    }
    public void RestartLevel() {
        SceneManager.LoadScene((Int32.Parse(SceneManager.GetActiveScene().name)));
    }
}
