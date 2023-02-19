using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AI_Npc : MonoBehaviour
{
    [NonSerialized]
    public bool isWalking;
    [NonSerialized]
    public bool isRunning;
    [NonSerialized]
    public bool isAttack;
    [NonSerialized]
    public bool isCanKill;
    [NonSerialized]
    public bool isIdle;

    private bool isPlayerCall;
    
    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    private State currentState;
    private TextMeshProUGUI statusText;
    public List<Transform> enemys;
    private GameObject particle;

    public void PlayParticle()
    {
        Instantiate(particle, transform.position + transform.forward, particle.transform.rotation);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        particle = GameManager.Instance.GetParticle();
        statusText = GetComponentInChildren<TextMeshProUGUI>();
        currentState = new Wander(gameObject, agent, player, statusText);
        statusText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (State.isGameEnd)
        {
            return;;
        }
        
        if (!GameManager.isGameStart)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= 5f && isPlayerCall)
        {
            isWalking = false;
            isPlayerCall = false;
            currentState = new Wander(gameObject, agent, player, statusText);
        }
        

        if (Input.GetKey(KeyCode.E))
        {
            isWalking = true;
            isPlayerCall = true;
            agent.ResetPath();
            agent.SetDestination(player.transform.position);
        }

        if (isPlayerCall)
        {
            return;
        }
        
        currentState = currentState.Process();
    }
}
