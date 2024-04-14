using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI highScoreText;
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

    private void OnEnable()
    {
        RemoteHighScoreManager.OnHighScoreChange += UpdateHighScoreText;
    }
    private void OnDisable()
    {
        RemoteHighScoreManager.OnHighScoreChange -= UpdateHighScoreText;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = RemoteHighScoreManager.Instance.highScoreData.Score.ToString();
    }

}
