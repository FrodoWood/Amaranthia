using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI upgradeName;   
    public TextMeshProUGUI upgradeDescription;
    public bool isSetup;
    
    public void SetUpgradeName(string name)
    {
        upgradeName.text = name;
    }
    public void SetUpgradeDescription(string description)
    {
        upgradeDescription.text = description;
    }

    public void CompleteSetup()
    {
        isSetup = true;
    }

    public bool IsSetup()
    {
        return isSetup;
    }
}
