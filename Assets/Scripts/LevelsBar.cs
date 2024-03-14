using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsBar : MonoBehaviour
{
    [SerializeField] private Image levelsBarSprite;
    [SerializeField] TextMeshProUGUI levelsText;

    private void Start()
    {

    }

    public void UpdateLevelsBar(float maxExp, float currentExp, int currentLevel)
    {
        levelsBarSprite.fillAmount = currentExp / maxExp;
        levelsText.text = "Level " + currentLevel;
    }
}
