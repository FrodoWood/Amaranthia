using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsBar : MonoBehaviour
{
    [SerializeField] private Image levelsBarSprite;
    [SerializeField] TextMeshProUGUI levelsText;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {

    }

    public void UpdateLevelsBar(float maxExp, float currentExp, int currentLevel, float totalExp)
    {
        levelsBarSprite.fillAmount = currentExp / maxExp;
        levelsText.text = "Level " + currentLevel;
        scoreText.text = totalExp.ToString();
    }
}
