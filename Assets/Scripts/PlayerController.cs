using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour
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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        input = new CustomActions();
        //AssignInputs();

        fireballAbility = new FireballAbility();
    }

    private void Start()
    {
        currentState = PlayerState.NotCasting;
    }
    private void AssignInputs()
    {
        //input.Main.Move.performed += ctx => ClickToMove();
        //input.Main.Q.performed += ctx => useFireballAbility();

    }

    private void changeState(PlayerState newState)
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
                    currentAbility.TriggerAbility(this);
                    StartCoroutine(waitXSecondsAndChangeState(0.3f, PlayerState.NotCasting));
                    castingAbility = true;
                }
                ClickToMove();
                break;

            case PlayerState.Dead:

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
        changeState(newState);
    }

    private void checkAbilityTrigger()
    {
        if (input.Main.Q.triggered)
        {
            changeState(PlayerState.Casting);
            changeAbility(fireballAbility);
        }
        else if (input.Main.W.triggered)
        {
            changeState(PlayerState.Casting);
        }
        else if (input.Main.E.triggered)
        {
            changeState(PlayerState.Casting);
        }
        else if (input.Main.R.triggered)
        {
            changeState(PlayerState.Casting);
        }
    }

    private void FaceNavMeshTarget()
    {
        if(agent.velocity != Vector3.zero)
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation;
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

}
