using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    private void Start()
    {
        if (!SaveLoadManager.instance.GameDataExists())
        {
            continueButton.interactable = false;
        }
    }
    public void OnNewGame()
    {
        SaveLoadManager.instance.NewGame();
        SceneManager.LoadSceneAsync(1);
    }

    public void OnContinueGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
