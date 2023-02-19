using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Pursuit : State
{
    public LineRenderer lineRenderer;
    private AI_Boss aiBoss;

    public Pursuit(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        stateName = StateStatus.Pursuit;
        agent.isStopped = false;
        agent.ResetPath();
        lineRenderer = npc.GetComponent<LineRenderer>();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Boss"))
        {
            npc.GetComponent<AI_Boss>().isWalking = true;
        }
        
        statusText.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        statusText.text = "<wiggle>Pursuit</wiggle>";
        base.Enter();
    }

    public override void Update()
    {
        if (isGameEnd) {return;}
        
        var targetDis = player.transform.position - npc.transform.position;
        var playerSpeed = player.GetComponent<CharacterController>().velocity.magnitude;
        var lookAhead = targetDis.magnitude / (agent.speed + playerSpeed);

        //Debug LookAhead
        if (PlayerController.isWalking)
        {
            agent.SetDestination(player.transform.position + player.transform.forward * lookAhead);
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }
        
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
            var aiBoss = npc.GetComponent<AI_Boss>();
            if (DistancePlayer() <= 1.6f)
            {
                nextState = new Attack(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (aiBoss.isCanDie && aiBoss.currentHealth <= 2)
            {
                nextState = new Evade(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
        }
    }

    public override void Exit()
    {
        if (npc.CompareTag("Boss"))
        {
            npc.GetComponent<AI_Boss>().isWalking = false;
        }
        
        lineRenderer.enabled = false;
        base.Exit();
    }
}