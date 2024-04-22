using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAbility2 : EnemyBaseAbility
{
    [SerializeField] private string abilityName;
    [SerializeField] private float fireballSpeed = 25f;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damage;
    [SerializeField] private GameObject witchFireballPrefab;

    private Vector3 fireballTargetPosition;

    protected override void Awake()
    {
        base.Awake();
        linkedEnemyState = EnemyState.Ability2;
    }

    protected override void Start()
    {
        base.Start();
        damage = baseDamage;
    }

    public override void TriggerAbility()
    {

        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility(); // Starts the cooldown timer and sets the ability on cooldown

        for(int i = 0; i < 6; i++)
        {
            float angle = i * (360f/6) * Mathf.Deg2Rad;

            Vector3 spawnDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Vector3 spawnPosition = transform.position + spawnDirection * 2f;
            Quaternion rotation = Quaternion.LookRotation(spawnDirection);
            GameObject newFireball = GameObject.Instantiate(witchFireballPrefab, spawnPosition + Vector3.up * 2, rotation);

            Fireball fireball = newFireball.GetComponent<Fireball>();
            fireball?.Setup(damage);

            Rigidbody fireballRigidbody = fireball.GetComponent<Rigidbody>();
            fireballRigidbody?.AddForce(newFireball.transform.forward * fireballSpeed, ForceMode.Impulse);
            
            GameObject.Destroy(newFireball, 2f);
        }

    }

    protected override void Update()
    {
        base.Update();
    }

}
