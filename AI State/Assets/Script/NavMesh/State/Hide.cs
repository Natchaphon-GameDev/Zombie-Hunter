using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Hide : State
{
    private LineRenderer lineRenderer;
    private GameObject[] hides;

    public Hide(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        stateName = StateStatus.Hide;
        agent.isStopped = false;
        agent.ResetPath();
        if (npc.CompareTag("Enemy"))
        {
            hides = npc.GetComponent<AI_Enemy>().hides;
        }
        lineRenderer = npc.GetComponent<LineRenderer>();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Enemy"))
        {
            agent.speed *= 1.5f;
            agent.angularSpeed += 50;
            npc.GetComponent<AI_Enemy>().isRuning = true;
        }
        
        statusText.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        statusText.text = "<wiggle>Hide</wiggle>";
        base.Enter();
    }

    public override void Update()
    {
        if (isGameEnd) {return;}
        
        var farFactor = 5f;
        
        var lastDis = Mathf.Infinity;
        var chosenSpot = Vector3.zero;
        
        for (var i = 0; i < hides.Length; i++)
        {
            var hideDir = hides[i].transform.position - player.transform.position;
            var hidePos = hides[i].transform.position + hideDir.normalized * farFactor;
        
            var dis = Vector3.Distance(npc.transform.position, hidePos);
            if (dis < lastDis)
            {
                chosenSpot = hidePos;
                lastDis = dis;
            }
        }
        
        agent.SetDestination(chosenSpot);
        
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
            if (agent.velocity.magnitude <= 0)
            {
                npc.GetComponent<AI_Enemy>().isIdle = true;
            }
            else
            {
                npc.GetComponent<AI_Enemy>().isIdle = false;
            }
            
            if (!TargetCanSeeMe())
            {
                nextState = new Seek(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (TargetCanSeeMe() && DistancePlayer() < 5)
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
            agent.speed /= 1.5f;
            agent.angularSpeed -= 50;
            npc.GetComponent<AI_Enemy>().isRuning = false;
            npc.GetComponent<AI_Enemy>().isIdle = false;
        }
        
        lineRenderer.enabled = false;
        base.Exit();
    }
}