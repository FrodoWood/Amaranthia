using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;
    private Camera cam;
    [SerializeField] bool isHudHealthbar;

    private void Start()
    {
        cam = Camera.main;
    }

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        healthbarSprite.fillAmount = currentHealth / maxHealth;
    }

    private void LateUpdate()
    {
        if (!isHudHealthbar)
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}
