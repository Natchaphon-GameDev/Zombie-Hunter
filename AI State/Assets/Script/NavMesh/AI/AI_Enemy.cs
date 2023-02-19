using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AI_Enemy : MonoBehaviour
{
    [NonSerialized]
    public bool isWalking;
    [NonSerialized]
    public bool isAttack;
    [NonSerialized]
    public bool isDie;
    [NonSerialized]
    public bool isCanKill;
    [NonSerialized]
    public bool isRuning;
    [NonSerialized] 
    public bool isIdle;
    
    public bool isCanDie;
    
    public GameObject[] hides;

    private NavMeshAgent agent;
    public Transform player;
    private State currentState;
    private TextMeshProUGUI statusText;

    private GameObject particle;

    private void OnDestroy()
    {
        if (!GameManager.Instance.isQuitting)
        {
            Instantiate(particle, transform.position, particle.transform.rotation);
        }
    }

    private void Start()
    {
        particle = GameManager.Instance.GetParticle();
        hides = GameObject.FindGameObjectsWithTag("hide");
        agent = GetComponent<NavMeshAgent>();
        statusText = GetComponentInChildren<TextMeshProUGUI>();
        currentState = new Seek(gameObject, agent, player, statusText);
        statusText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (State.isGameEnd)
        {
            agent.ResetPath();
            return;
        }

        if (isDie)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            return;
        }
        
        currentState = currentState.Process();
    }
}
