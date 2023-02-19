using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Heal : State
{
    private float timer;

    public Heal(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        
        stateName = StateStatus.Heal;
        agent.isStopped = true;
        agent.ResetPath();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Boss"))
        {
            timer = npc.GetComponent<AI_Boss>().regenHealthPerSec;
            npc.GetComponent<AI_Boss>().isIdle = true;
        }
        
        statusText.gameObject.SetActive(true);
        statusText.text = "<wiggle>Heal</wiggle>";
        base.Enter();
    }

    public override void Update()
    {
        if (isGameEnd) {return;}
        
        if (npc.CompareTag("Boss"))
        {
            var aiBoss = npc.GetComponent<AI_Boss>();
            timer -= Time.deltaTime;

            if (timer <= 0 && aiBoss.currentHealth < aiBoss.GetMaxHP())
            {
                aiBoss.currentHealth++;
                timer += aiBoss.regenHealthPerSec;
            }
            
            //Change State
            if (aiBoss.isCanDie && DistancePlayer() < 5)
            {
                nextState = new Pursuit(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (aiBoss.isCanDie && DistancePlayer() >= 5)
            {
                nextState = new CleaverHide(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (TargetCanSeeMe() && DistancePlayer() < 3)
            {
                nextState = new Evade(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
            else if (aiBoss.currentHealth == aiBoss.GetMaxHP())
            {
                nextState = new Pursuit(npc, agent, player, statusText);
                stateEvent = EventState.Exit;
            }
        }
    }

    public override void Exit()
    {
        if (npc.CompareTag("Boss"))
        {
            npc.GetComponent<AI_Boss>().isIdle = false;
        }
        base.Exit();
    }
}