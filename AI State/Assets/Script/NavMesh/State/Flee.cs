using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Flee : State
{
    private LineRenderer lineRenderer;

    public Flee(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        stateName = StateStatus.Flee;
        agent.isStopped = false;
        agent.ResetPath();
        lineRenderer = npc.GetComponent<LineRenderer>();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Enemy"))
        {
            agent.speed *= 2;
            agent.angularSpeed += 100;
            npc.GetComponent<AI_Enemy>().isRuning = true;
        }
        
        statusText.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        statusText.text = "<wiggle>Flee</wiggle>";
        base.Enter();
    }

    public override void Update()
    {
        if (isGameEnd) {return;}
        
        agent.SetDestination(npc.transform.position - player.transform.position + npc.transform.position);
        
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

        if (npc.CompareTag("Enemy"))
        {
            if (npc.GetComponent<AI_Enemy>().isCanDie && DistancePlayer() > 5)
            {
                nextState = new Hide(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (!npc.GetComponent<AI_Enemy>().isCanDie && DistancePlayer() <= 5)
            {
                nextState = new Seek(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
        }
    }

    public override void Exit()
    {
        if (npc.CompareTag("Enemy"))
        {
            agent.speed /= 2;
            agent.angularSpeed -= 100;
            npc.GetComponent<AI_Enemy>().isRuning = false;
        }
        
        lineRenderer.enabled = false;
        base.Exit();
    }
}