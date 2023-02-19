using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Seek : State
{
    public LineRenderer lineRenderer;

    public Seek(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        stateName = StateStatus.Seek;
        agent.isStopped = false;
        agent.ResetPath();
        lineRenderer = npc.GetComponent<LineRenderer>();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Enemy"))
        {
            npc.GetComponent<AI_Enemy>().isWalking = true;
        }
        else if (npc.CompareTag("Npc"))
        {
            npc.GetComponent<AI_Npc>().isRunning = true;
        }
        
        statusText.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        statusText.text = "<wiggle>Seek</wiggle>";
        base.Enter();
    }

    public override void Update()
    {
        if (isGameEnd) {return;}

        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        
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
        }

        if (npc.CompareTag("Enemy"))
        {
            if (DistancePlayer() <= 1.4f)
            {
                nextState = new Attack(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (npc.GetComponent<AI_Enemy>().isCanDie)
            {
                nextState = new Flee(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
        }
        else if (npc.CompareTag("Npc"))
        {
            if (player == null)
            {
                npc.GetComponent<AI_Npc>().enemys.Remove(player);
                nextState = new Wander(npc, agent, null, statusText);
                stateEvent = EventState.Exit;
            }
            else if (DistancePlayer() <= 1.4f)
            {
                nextState = new Attack(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
        }
    }

    public override void Exit()
    {
        if (npc.CompareTag("Enemy"))
        {
            npc.GetComponent<AI_Enemy>().isWalking = false;
        }
        else if (npc.CompareTag("Npc"))
        {
            npc.GetComponent<AI_Npc>().isRunning = false;
        }
        
        lineRenderer.enabled = false;
        base.Exit();
    }
}