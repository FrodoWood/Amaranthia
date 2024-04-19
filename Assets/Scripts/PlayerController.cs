using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour, IDamageable, ISaveable
{
    CustomActions input;
    NavMeshAgent agent;
    Animator animator;
    PlayerStats playerStats;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    private PlayerState currentState;
    private AnimatorStateInfo animatorStateInfo;

    [Header("Abilities")]
    [SerializeField] private UIAbility uiAbility;
    public Transform projectileSpawnPoint;
    private AbilityQ abilityQ;
    private AbilityW abilityW;
    private AbilityE abilityE;
    private AbilityR abilityR;

    [Header("Health")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private Healthbar hudHealthbar;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        input = new CustomActions();
        playerStats = GetComponent<PlayerStats>();
        abilityQ = GetComponent<AbilityQ>();
        abilityW = GetComponent<AbilityW>();
        abilityE = GetComponent<AbilityE>();
        abilityR = GetComponent<AbilityR>();

    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.UpdateHealthbar(maxHealth, currentHealth);
        hudHealthbar.UpdateHealthbar(maxHealth, currentHealth);

        BaseAbility[] abilities = new BaseAbility[] { abilityQ, abilityW, abilityE, abilityR };
        uiAbility.Initialise(abilities);

        ChangeState(PlayerState.Idle);
    }
    private void Update()
    {
        UpdateState(currentState);

        UpdateStats();
    }

    private void UpdateStats()
    {
        agent.speed = playerStats.movementSpeed;
    }
    private void ChangeState(PlayerState newState)
    {
        OnExitState(currentState);
        currentState = newState;
        OnEnterState(currentState);
    }


    private void OnEnterState(PlayerState currentState)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                OnEnterIdle();
                break;
            case PlayerState.Moving:
                OnEnterMoving();
                break;
            case PlayerState.Dead:
                OnEnterDead();
                break;
            case PlayerState.Q:
                OnEnterQ();
                break;
            case PlayerState.W:
                OnEnterW();
                break;
            case PlayerState.E:
                OnEnterE();
                break;
            case PlayerState.R:
                OnEnterR();
                break;
        }
    }
    private void OnExitState(PlayerState currentState)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                OnExitIdle();
                break;
            case PlayerState.Moving:
                OnExitMoving();
                break;
            case PlayerState.Dead:
                OnExitDead();
                break;
            case PlayerState.Q:
                OnExitQ();
                break;
            case PlayerState.W:
                OnExitW();
                break;
            case PlayerState.E:
                OnExitE();
                break;
            case PlayerState.R:
                OnExitR();
                break;
        }
    }
    private void UpdateState(PlayerState currentState)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Dead:
                UpdateDead();
                break;
            case PlayerState.Q:
                UpdateQ();
                break;
            case PlayerState.W:
                UpdateW();
                break;
            case PlayerState.E:
                UpdateE();
                break;
            case PlayerState.R:
                UpdateR();
                break;
        }
    }



    private void OnEnterIdle()
    {
        animator.SetTrigger("Idle");
    }
    private void UpdateIdle()
    {
        ClickToMove();
        if (CheckAbilityTrigger()) HandleAbilityTrigger();
        else if(agent.hasPath) ChangeState(PlayerState.Moving);
    }
    private void OnExitIdle()
    {
        
    }



    private void OnEnterMoving()
    {
        animator.SetTrigger("Running");
    }
    private void UpdateMoving()
    {
        ClickToMove();
        if (CheckAbilityTrigger()) HandleAbilityTrigger();
        else if(!agent.hasPath) ChangeState(PlayerState.Idle);
    }
    private void OnExitMoving()
    {
        
    }

    private void OnEnterQ()
    {
        faceMouse();
        animator.SetTrigger("QAbility");
        agent.isStopped = true;
    }
    private void UpdateQ()
    {
        ClickToMove();

        //Check if the current animation has finished playing
        if (CurrentAnimationFinished())
        {
            if (CheckAbilityTrigger()) HandleAbilityTrigger();
            else if (!agent.hasPath) ChangeState(PlayerState.Idle);
            else if (agent.hasPath) ChangeState(PlayerState.Moving);
        }
    }
    private void OnExitQ()
    {
        agent.isStopped = false;
    }
    public void TriggerQAbility()
    {
        abilityQ.TriggerAbility();
    }

    private void OnEnterW()
    {
        animator.SetTrigger("WAbility");
        agent.isStopped = true;
    }
    private void UpdateW()
    {
        ClickToMove();
        if (CurrentAnimationFinished())
        {
            if (CheckAbilityTrigger()) HandleAbilityTrigger();
            else if (!agent.hasPath) ChangeState(PlayerState.Idle);
            else if (agent.hasPath) ChangeState(PlayerState.Moving);
        }
    }
    private void OnExitW()
    {
        agent.isStopped = false;
    }
    public void TriggerWAbility()
    {
        abilityW.TriggerAbility();
    }

    private void OnEnterE()
    {
        animator.SetTrigger("EAbility");
        abilityE.TriggerAbility();
        agent.isStopped = true;
    }
    private void UpdateE()
    {
        ClickToMove();
        if (CurrentAnimationFinished())
        {
            if (CheckAbilityTrigger()) HandleAbilityTrigger();
            else if (!agent.hasPath) ChangeState(PlayerState.Idle);
            else if (agent.hasPath) ChangeState(PlayerState.Moving);
        }
    }
    private void OnExitE()
    {
        agent.isStopped = false;
    }


    private void OnEnterR()
    {
        animator.SetTrigger("RAbility");
        abilityR.TriggerAbility();
        agent.isStopped = true;
    }
    private void UpdateR()
    {
        ClickToMove();
        if (CurrentAnimationFinished())
        {
            if (CheckAbilityTrigger()) HandleAbilityTrigger();
            else if (!agent.hasPath) ChangeState(PlayerState.Idle);
            else if (agent.hasPath) ChangeState(PlayerState.Moving);
        }
    }
    private void OnExitR()
    {
        agent.isStopped = false;
    }
    public void TriggerRAbility()
    {
        abilityR.TriggerAbility();
    }

    private void OnEnterDead()
    {
        animator.SetTrigger("Dead");
        agent.enabled = false;
        Collider coll= GetComponent<Collider>();
        if(coll != null) coll.enabled = false;
    }
    private void UpdateDead()
    {
        
    }
    private void OnExitDead()
    {
        
    }


    private void ClickToMove()
    {
        if (!input.Main.Move.triggered)
        {
            return;
        }
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
        {
            agent.destination = hit.point;
            if(clickEffect != null)
            {
                Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            }
        }
    }


    private IEnumerator ChangeStateAfterSeconds(float amount, PlayerState newState)
    {
        Debug.Log("Waiting for " +  amount + " seconds");
        yield return new WaitForSeconds(amount);
        Debug.Log("Changing state to " + newState);
        ChangeState(newState);
    }

    private bool CheckAbilityTrigger()
    {
        return (input.Main.Q.triggered || input.Main.W.triggered || input.Main.E.triggered || input.Main.R.triggered) ? true: false;
    }

    private void HandleAbilityTrigger()
    {
        if (input.Main.Q.triggered && abilityQ.Ready() && abilityQ.isEnabled())
        {
            ChangeState(PlayerState.Q);
        }
        else if (input.Main.W.triggered && abilityW.Ready() && abilityW.isEnabled())
        {
            ChangeState(PlayerState.W);
        }
        else if (input.Main.E.triggered && abilityE.Ready() && abilityE.isEnabled())
        {
            ChangeState(PlayerState.E);
        }
        else if (input.Main.R.triggered && abilityR.Ready() && abilityR.isEnabled())
        {
            ChangeState(PlayerState.R);
        }
    }


    private void FaceNavMeshTarget()
    {
        if(agent.velocity != Vector3.zero)
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 35f * Time.deltaTime);
        }
    }

    private void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayers))
        {
            Vector3 direction = hit.point - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation;
        }
    }

    private void SetAnimations()
    {
        
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void TakeDamage(float amount, EntityType entityType)
    {
        if (entityType == EntityType.Allied) return;
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        //Update UI
        healthbar.UpdateHealthbar(maxHealth, currentHealth);
        hudHealthbar.UpdateHealthbar(maxHealth, currentHealth);

        Debug.Log($"Player has taken {amount} damage!");

        if (currentHealth <= 0)
        {
            ChangeState(PlayerState.Dead);
        }
    }

    public void LoadData(GameData data)
    {
        agent.Warp(data.playerPosition);
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }

    private bool CurrentAnimationFinished()
    {
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.normalizedTime >= 1 && !animator.IsInTransition(0)) return true;
        else return false;
    }
}
