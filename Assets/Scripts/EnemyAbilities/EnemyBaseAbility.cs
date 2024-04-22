using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseAbility : MonoBehaviour, IAbility
{
    private bool isComplete;
    [SerializeField] protected bool abilityEnabled = false;
    [SerializeField] protected bool onCooldown;
    [SerializeField] public float baseCooldown;
    [SerializeField] public float cooldown;
    public float cooldownTimer { get; private set; }

    [SerializeField] protected float duration;
    private float durationTimer;

    protected Enemy enemy;
    protected EnemyState linkedEnemyState;

    protected virtual void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    protected virtual void Start()
    {
        onCooldown = false;
        cooldown = baseCooldown;
    }

    protected virtual void Update()
    {
        UpdateCooldown();
        UpdateDuration();
    }

    public virtual void TriggerAbility()
    {
        onCooldown = true;
        isComplete = false;
        cooldownTimer = cooldown;
        durationTimer = duration;
    }

    private void UpdateCooldown()
    {
        if (!onCooldown) return;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            cooldownTimer = 0;
            onCooldown = false;
        }
    }
    private void UpdateDuration()
    {
        if (isComplete) return;

        durationTimer -= Time.deltaTime;
        if (durationTimer <= 0)
        {
            durationTimer = 0;
            isComplete = true;
        }
    }

    public bool Ready()
    {
        return !onCooldown;
    }

    public bool Complete()
    {
        return isComplete;
    }

    public bool isEnabled()
    {
        return abilityEnabled;
    }

    public EnemyState GetAbilityState()
    {
        return linkedEnemyState;
    }
}
