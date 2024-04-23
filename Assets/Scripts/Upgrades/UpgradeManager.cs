using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> upgradesPool;

    private PlayerStats playerStats;
    public Transform upgradeUIParent;
    public GameObject upgradeUIPrefab;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        upgradesPool = FindObjectsOfType<Upgrade>().ToList();
    }
    void Start()
    {
        foreach(Upgrade upgrade in upgradesPool)
        {
            if (upgrade.isActivated)
            {
                upgrade.Initialise(playerStats);
            }
        }
    }

    void Update()
    {
    }

    [ContextMenu("Display upgrades")]
    public void DisplayUpgradeChoices()
    {
        GameObject upgradeUI = Instantiate(upgradeUIPrefab, upgradeUIParent);
        List<UpgradeButton> upgradeButtons = upgradeUI.GetComponentsInChildren<UpgradeButton>().ToList();
        Debug.Log("Thre are " +  upgradeButtons.Count + "buttons");

        List<Upgrade> inactiveUpgrades = upgradesPool.Where(upgrade => !upgrade.isActivated).ToList();
        inactiveUpgrades = inactiveUpgrades.OrderBy(upgrade => Random.value).ToList();

        for (int i = 0; i<Mathf.Min(3, inactiveUpgrades.Count); i++)
        {
            Upgrade upgrade = inactiveUpgrades[i];
            UpgradeButton upgradeButton = upgradeButtons[i];
            upgradeButton?.SetUpgradeName(upgrade.GetName());
            upgradeButton?.SetUpgradeDescription(upgrade.GetDescription());
            upgradeButton.GetComponentInChildren<Button>().onClick.AddListener(() => ActivateUpgrade(upgrade));
            upgradeButton?.CompleteSetup();
        }

        for (int i = 0; i < 3; i++)
        {
            UpgradeButton upgradeButton = upgradeButtons[i];
            if(!upgradeButton.IsSetup())
            {
                Destroy(upgradeButton.gameObject);
            }
        }
    }

    public void ActivateUpgrade(Upgrade upgrade)
    {
        if (!upgradesPool.Contains(upgrade)) return;
        if (upgrade.isActivated) return;
        upgrade.Initialise(playerStats);
        upgrade.isActivated = true;

        for (int i = upgradeUIParent.childCount - 1; i >= 0; i--)
        {
            GameObject upgradeUIGameObject = upgradeUIParent.GetChild(i).gameObject;
            Destroy(upgradeUIGameObject);
        }
    }

    private void OnEnable()
    {
        LevelsManager.OnLevelUp += DisplayUpgradeChoices;
    }

    private void OnDisable()
    {
        LevelsManager.OnLevelUp -= DisplayUpgradeChoices;
    }
}
