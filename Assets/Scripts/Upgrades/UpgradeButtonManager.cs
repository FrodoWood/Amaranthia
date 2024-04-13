using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButtonManager : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public GameObject upgradeButtonPrefab;
    public Transform buttonContainer;
    public float buttonSpacing = 10f;
    void Start()
    {
        float yOffset = 0f;
        foreach (Upgrade upgrade in upgradeManager.upgradesPool)
        {
            GameObject newButton = Instantiate(upgradeButtonPrefab, buttonContainer);
            Button button = newButton.GetComponent<Button>();

            TextMeshProUGUI textButton = button.GetComponentInChildren<TextMeshProUGUI>();
            textButton.text = upgrade.GetName();

            RectTransform buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(0f, yOffset);
            yOffset -= (buttonRect.sizeDelta.y + buttonSpacing);

            button.onClick.AddListener(() => upgradeManager.ActivateUpgrade(upgrade));
        }
    }

    void Update()
    {
        
    }
}
