using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    private State currentState;
    private TextMeshProUGUI statusText;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        statusText = GetComponentInChildren<TextMeshProUGUI>();
        currentState = new Wander(gameObject, agent, player, statusText);
    }

    private void Update()
    {
        transform.GetChild(0).LookAt(Camera.main.transform);
        currentState = currentState.Process();
    }

}