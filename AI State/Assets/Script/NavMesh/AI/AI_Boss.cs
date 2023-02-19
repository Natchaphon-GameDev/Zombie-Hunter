using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI_Boss : MonoBehaviour
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

    public bool damaged;
   
    private GameObject particle;

    public bool isCanDie;

    [SerializeField] private int maxHealth;
    public float regenHealthPerSec;

    public int GetMaxHP()
    {
        return maxHealth;
    }

    private void OnDestroy()
    {
        if (!GameManager.Instance.isQuitting)
        {
            Instantiate(particle, transform.position, particle.transform.rotation);
        }
    }

    [SerializeField] private Image healthBar;

    [NonSerialized]
    public int currentHealth;

    public GameObject[] hides;

    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    private State currentState;
    private TextMeshProUGUI statusText;

    private void Start()
    {
        currentHealth = maxHealth;
        particle = GameManager.Instance.GetParticle();
        hides = GameObject.FindGameObjectsWithTag("hide");
        agent = GetComponent<NavMeshAgent>();
        statusText = GetComponentInChildren<TextMeshProUGUI>();
        currentState = new Pursuit(gameObject, agent, player, statusText);
        statusText.gameObject.SetActive(false);
    }
    

    private void Update()
    {
        if (!GameManager.isGameStart)
        {
            return;
        }
        if (State.isGameEnd)
        {
            agent.ResetPath();
            return;
        }

        if (damaged && currentHealth >= 2)
        {
            agent.ResetPath();
            return;
        }
        
        //HealthBar Handle
        healthBar.fillAmount = (float)currentHealth / maxHealth;

        if (isDie)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            return;
        }
        currentState = currentState.Process();
    }
}
