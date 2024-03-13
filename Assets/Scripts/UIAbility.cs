using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbility : MonoBehaviour
{
    //[SerializeField] private Image qAbility;
    //[SerializeField] private Image wAbility;
    //[SerializeField] private Image eAbility;
    //[SerializeField] private Image rAbility;

    [SerializeField] private Image[] abilityImages;
    private BaseAbility[] abilities;

    //private AbilityQ abilityQ;
    //private AbilityW abilityW;
    //private AbilityE abilityE;
    //private AbilityR abilityR;

    public void Initialise(BaseAbility[] _abilities)
    {
        abilities = _abilities;
    }

    private void Update()
    {
        if(abilities == null || abilityImages.Length != abilities.Length) 
        {
            Debug.LogError("Hud ability images amount mismatch with abilities");
            return;
        }

        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] != null)
            {
                abilityImages[i].fillAmount = 1- (abilities[i].cooldownTimer / abilities[i].cooldown);
            }
        }
    }



    //public void UpdateAbilityCooldownFill(AbilityType abilityType, float currentTimer, float maxCooldown)
    //{
    //    switch (abilityType)
    //    {
    //        case AbilityType.Q:
    //            qAbility.fillAmount = currentTimer / maxCooldown;
    //            break;
    //        case AbilityType.W:
    //            wAbility.fillAmount = currentTimer / maxCooldown;
    //            break;
    //        case AbilityType.E:
    //            eAbility.fillAmount = currentTimer / maxCooldown;
    //            break;
    //        case AbilityType.R:
    //            rAbility.fillAmount = currentTimer / maxCooldown;
    //            break;
    //    }

    //}
}
