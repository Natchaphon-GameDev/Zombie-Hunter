using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CleaverHide : State
{
    private LineRenderer lineRenderer;
    private GameObject[] hides;
    public CleaverHide(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        stateName = StateStatus.CleverHide;
        agent.isStopped = false;
        agent.ResetPath();
        if (npc.CompareTag("Boss"))
        {
            hides = npc.GetComponent<AI_Boss>().hides;
        }
        lineRenderer = npc.GetComponent<LineRenderer>();
    }
    
    public override void Enter()
    {
        if (npc.CompareTag("Boss"))
        {
            agent.speed *= 2f;
            agent.angularSpeed += 200;
            npc.GetComponent<AI_Boss>().isRuning = true;
        }
        
        statusText.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        statusText.text = "<wiggle>Clever Hide</wiggle>";
        base.Enter();
    }
    
    public override void Update()
    {
        if (isGameEnd) {return;}

        var farFactor = 20f;
        
        var lastDis = Mathf.Infinity;
        var chosenSpot = Vector3.zero;
        var chosenDir = Vector3.zero;
        var chosenHide = hides[0];
        
        for (var i = 0; i < hides.Length; i++)
        {
            var hideDir = hides[i].transform.position - player.transform.position;
            var hidePos = hides[i].transform.position + hideDir.normalized * farFactor;
        
            var dis = Vector3.Distance(npc.transform.position, hidePos);
            if (dis < lastDis)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
        
                chosenHide = hides[i];
                lastDis = dis;
            }
        }

        var hideCol = chosenHide.GetComponent<Collider>();
        var backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        var distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);
        
        agent.SetDestination(info.point + chosenDir.normalized);
        
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
            if (agent.velocity.magnitude <= 0 && !npc.GetComponent<AI_Boss>().isCanDie)
            {
                nextState = new Heal(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (TargetCanSeeMe() && DistancePlayer() <= 1.5f)
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
            agent.speed /= 2f;
            agent.angularSpeed -= 200;
            npc.GetComponent<AI_Boss>().isRuning = false;
        }
        
        lineRenderer.enabled = false;
        base.Exit();
    }

}