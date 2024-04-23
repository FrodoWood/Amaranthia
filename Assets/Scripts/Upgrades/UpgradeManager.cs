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
        for(int i = 0; i<Mathf.Min(3, upgradesPool.Count); i++)
        {
            Upgrade upgrade = upgradesPool[i];
            upgradesPool = upgradesPool.OrderBy(upgrade => Random.value).ToList();
            GameObject upgradeUI = Instantiate(upgradeUIPrefab, upgradeUIParent);
            upgradeUI.GetComponentInChildren<TextMeshProUGUI>().text = upgrade.GetName();
            Vector3 newPosition = upgradeUIParent.position + Vector3.right * i * 100f;
            upgradeUI.transform.position = newPosition;
            upgradeUI.GetComponentInChildren<Button>().onClick.AddListener(() => ActivateUpgrade(upgrade));

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
}
