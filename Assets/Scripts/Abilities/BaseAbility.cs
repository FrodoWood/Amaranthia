using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility: MonoBehaviour, IAbility
{
    [SerializeField] protected bool onCooldown;
    private bool isComplete;
    [SerializeField] protected bool abilityEnabled = false;
    [SerializeField] public float cooldown;
    [SerializeField] protected float duration;
    public float cooldownTimer { get; private set; }
    private float durationTimer;
    protected PlayerController player;

    protected virtual void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    protected virtual void Start()
    {
        onCooldown = false;
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
}
