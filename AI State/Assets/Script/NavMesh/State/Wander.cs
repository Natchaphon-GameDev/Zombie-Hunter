using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Wander : State
{
    public LineRenderer lineRenderer;
    private Vector3 wanderTarget = Vector3.zero;
    
    private float nearestEnemy;
    private Transform enemyTemp;


    public Wander(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        nearestEnemy = Mathf.Infinity;
        stateName = StateStatus.Wander;
        agent.isStopped = false;
        agent.ResetPath();
        lineRenderer = npc.GetComponent<LineRenderer>();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Npc"))
        {
            npc.GetComponent<AI_Npc>().isWalking = true;
        }
        
        statusText.gameObject.SetActive(true);
        lineRenderer.enabled = true;
        statusText.gameObject.SetActive(true);
        statusText.text = "<wiggle>Wander</wiggle>";
        base.Enter();
    }

    public override void Update()
    {
        if (isGameEnd) {return;}
        
        var wanderRadius = 10f;
        var wanderDistance = 10f;
        var wanderJitter = 1;
        
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0,
            Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        var targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        var targetWorld = npc.gameObject.transform.InverseTransformVector(targetLocal);
        
        agent.SetDestination(targetWorld);
        
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

        if (npc.CompareTag("Npc"))
        {
            var aiNpc = npc.GetComponent<AI_Npc>();

            if (aiNpc.enemys.Count != 0)
            {
                foreach (var enemy in aiNpc.enemys)
                {
                    if (enemy == null)
                    {
                        aiNpc.enemys.Remove(enemy);
                        break;
                    }
                    
                    var disTemp = Vector3.Distance(enemy.transform.position, npc.transform.position);
                    
                    if (disTemp < nearestEnemy)
                    {
                        nearestEnemy = disTemp;
                        enemyTemp = enemy;
                    }
                }
            }
            
            if (enemyTemp != null)
            {
                if (Vector3.Distance(enemyTemp.transform.position,npc.transform.position) <= 6f && enemyTemp != null)
                {
                    nextState = new Seek(npc, agent, enemyTemp, statusText);
                    stateEvent = EventState.Exit;
                }
            }

            // else if (DistancePlayer() < 7 && CanSeePlayer())
            // {
            //     nextState = new Seek(npc, agent, player, statusText);
            //     stateEvent = EventState.Exit;
            // }
        }
    }

    public override void Exit()
    {
        if (npc.CompareTag("Npc"))
        {
            npc.GetComponent<AI_Npc>().isWalking = false;
        }
        
        lineRenderer.enabled = false;
        base.Exit();
    }
}