using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour, IDamageable
{
    CustomActions input;
    NavMeshAgent agent;
    Animator animator;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    private PlayerState currentState;

    [Header("Abilities")]
    private BaseAbility currentAbility;
    public BaseAbility fireballAbility;
    private bool castingAbility = false;
    public GameObject fireballPrefab;

    [Header("Health")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        input = new CustomActions();
        //AssignInputs();

    }

    private void Start()
    {
        fireballAbility = new FireballAbility();
        currentState = PlayerState.NotCasting;
        currentAbility = fireballAbility;
        currentHealth = maxHealth;
    }
    private void AssignInputs()
    {
        //input.Main.Move.performed += ctx => ClickToMove();
        //input.Main.Q.performed += ctx => useFireballAbility();

    }

    private void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }

    private void changeAbility(BaseAbility newAbility)
    {
        currentAbility = newAbility;
    }
    private void useFireballAbility()
    {
        
        fireballAbility.TriggerAbility(this);
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

    private void Update()
    {
        //Debug.Log(currentState);
        switch (currentState)
        {
            case PlayerState.NotCasting:
                castingAbility = false;
                ClickToMove();
                checkAbilityTrigger();
                agent.isStopped = false;
                FaceNavMeshTarget();
                break;

            case PlayerState.Casting:
                agent.isStopped = true;
                if (castingAbility == false)
                {
                    faceMouse();
                    currentAbility?.TriggerAbility(this);
                    StartCoroutine(waitXSecondsAndChangeState(0.3f, PlayerState.NotCasting));
                    castingAbility = true;
                }
                ClickToMove();
                break;

            case PlayerState.Dead:
                agent.enabled = false;

                break;
        }
        //FaceTarget();
        //SetAnimations();
    }

    private IEnumerator waitXSecondsAndChangeState(float amount, PlayerState newState)
    {
        Debug.Log("Waiting for " +  amount + " seconds");
        yield return new WaitForSeconds(amount);
        Debug.Log("Changing state to " + newState);
        ChangeState(newState);
    }

    private void checkAbilityTrigger()
    {
        if (input.Main.Q.triggered)
        {
            ChangeState(PlayerState.Casting);
            changeAbility(fireballAbility);
        }
        else if (input.Main.W.triggered)
        {
            ChangeState(PlayerState.Casting);
        }
        else if (input.Main.E.triggered)
        {
            ChangeState(PlayerState.Casting);
        }
        else if (input.Main.R.triggered)
        {
            ChangeState(PlayerState.Casting);
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
        Debug.Log($"Player has taken {amount} damage!");

        if (currentHealth == 0)
        {
            ChangeState(PlayerState.Dead);
        }
    }
}
