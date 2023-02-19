using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Evade : State
{
    private LineRenderer lineRenderer;

    public Evade(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        stateName = StateStatus.Evade;
        agent.isStopped = false;
        agent.ResetPath();
        lineRenderer = npc.GetComponent<LineRenderer>();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Boss"))
        {
            agent.speed *= 3;
            agent.angularSpeed += 100;
            npc.GetComponent<AI_Boss>().isRuning = true;
        }
        
        statusText.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        statusText.text = "<wiggle>Evade</wiggle>";
        base.Enter();
    }

    public override void Update()
    {
        if (isGameEnd) {return;}
        
        var targetDis = player.transform.position - npc.transform.position;
        //Change Navmesh to CharacterController
        var playerSpeed = player.GetComponent<CharacterController>().velocity.magnitude;
        var lookAhead = targetDis.magnitude / (agent.speed + playerSpeed);
        
        agent.SetDestination(npc.transform.position - (player.transform.position + player.transform.forward * lookAhead) + npc.transform.position);

        if (GameManager.Instance.showLineAgent)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0,agent.destination);
            lineRenderer.SetPosition(1,npc.transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
        
        if (npc.CompareTag("Boss"))
        {
            if (DistancePlayer() >= 3)
            {
                nextState = new CleaverHide(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
        }
    }

    public override void Exit()
    {
        if (npc.CompareTag("Boss"))
        {
            agent.speed /= 3;
            agent.angularSpeed -= 100;
            npc.GetComponent<AI_Boss>().isRuning = false;
        }
        lineRenderer.enabled = false;
        base.Exit();
    }
}