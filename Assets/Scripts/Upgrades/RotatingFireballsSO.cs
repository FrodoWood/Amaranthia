using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(menuName = "Upgrades/Ability Upgrade")]
public class RotatingFireballsSO : UpgradeSO
{
    public int fireballAmount = 1;
    public float rotationSpeed = 1;
    public float radius = 10;
    public GameObject fireballPrefab;
    private GameObject centre;
    public override void Initialise(PlayerStats _playerStats)
    {
        base.Initialise(_playerStats);
        centre = Instantiate(new GameObject("FireallAbilityCenter"), GameManager.instance.player.transform.position, Quaternion.identity);
        for (int i = 0; i < fireballAmount; i++)
        {
            float angle = i * 360/fireballAmount;
            float x = centre.transform.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = centre.transform.position.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 newFireballPosition = new Vector3(x, centre.transform.position.y, z);

            GameObject newFireball = Instantiate(fireballPrefab, newFireballPosition, Quaternion.identity, centre.transform);
            newFireball.transform.localScale *= 2;
            Fireball fireball = newFireball.GetComponent<Fireball>();
            fireball?.Setup(_playerStats.abilityDamage);
        }

    }

    public override void OnUpdate()
    {
        centre.transform.position = GameManager.instance.player.transform.position;
        centre.transform.localEulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);
    }
}
