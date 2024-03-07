using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{
    Q,
    W,
    E,
    R
}
public class UIAbility : MonoBehaviour
{
    [SerializeField] private Image qAbility;
    [SerializeField] private Image wAbility;
    [SerializeField] private Image eAbility;
    [SerializeField] private Image rAbility;

    public void UpdateAbilityCooldownFill(AbilityType abilityType, float currentTimer, float maxCooldown)
    {
        switch (abilityType)
        {
            case AbilityType.Q:
                qAbility.fillAmount = currentTimer / maxCooldown;
                break;
            case AbilityType.W:
                wAbility.fillAmount = currentTimer / maxCooldown;
                break;
            case AbilityType.E:
                eAbility.fillAmount = currentTimer / maxCooldown;
                break;
            case AbilityType.R:
                rAbility.fillAmount = currentTimer / maxCooldown;
                break;
        }

    }
}
