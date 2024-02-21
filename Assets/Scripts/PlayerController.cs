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

    float lookRotationSpeed = 8f;

    private PlayerState currentState;

    public BaseAbility fireballAbility;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        input = new CustomActions();
        AssignInputs();

        fireballAbility = new FireballAbility();
    }

    private void Start()
    {
        currentState = PlayerState.Moving;
    }
    private void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
        input.Main.Q.performed += ctx => useFireballAbility();

    }

    private void changeState(PlayerState newState)
    {
        currentState = newState;
    }
    private void useFireballAbility()
    {
        
        fireballAbility.TriggerAbility(this);
    }
    private void ClickToMove()
    {
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
        faceMouse();
        //switch (currentState)
        //{
        //    case PlayerState.Moving:
        //        FaceTarget();
        //        break;
        //    case PlayerState.Casting:
        //        faceMouse();
        //        break;
        //}
        //FaceTarget();
        //SetAnimations();
    }
    private void FaceTarget()
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
