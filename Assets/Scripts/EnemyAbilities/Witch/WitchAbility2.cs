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

        fireballTargetPosition = transform.position + Random.insideUnitSphere * 10;
        GameObject newFireball = GameObject.Instantiate(witchFireballPrefab, fireballTargetPosition + Vector3.up * 30, Quaternion.identity);

        Fireball fireball = newFireball.GetComponent<Fireball>();
        fireball?.Setup(damage);

        Rigidbody fireballRigidbody = fireball.GetComponent<Rigidbody>();
        fireballRigidbody?.AddForce(Vector3.down * fireballSpeed, ForceMode.Impulse);

        GameObject.Destroy(newFireball, 2f);


    }

    protected override void Update()
    {
        base.Update();
    }

}
