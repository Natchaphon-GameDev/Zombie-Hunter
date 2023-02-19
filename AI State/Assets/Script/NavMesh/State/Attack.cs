using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    private float timer;
    public Attack(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI statusText) : base(npc, agent, player, statusText)
    {
        stateName = StateStatus.Attack;
        agent.isStopped = true;
        agent.ResetPath();
    }

    public override void Enter()
    {
        if (npc.CompareTag("Enemy"))
        {
            npc.GetComponent<AI_Enemy>().isAttack = true;
        }
        else if (npc.CompareTag("Boss"))
        {
            npc.GetComponent<AI_Boss>().isAttack = true;
        }
        else if (npc.CompareTag("Npc"))
        {
            npc.GetComponent<AI_Npc>().isAttack = true;
            timer = 1.5f;
        }
        
        statusText.gameObject.SetActive(true);
        statusText.text = "<wiggle>Attack</wiggle>";
        base.Enter();
    }
    
    public override void Update()
    {
        if (isGameEnd) {return;}

        
        if (npc.CompareTag("Enemy"))
        {
            if (npc.GetComponent<AI_Enemy>().isCanKill)
            {
                isGameEnd = true;
            }
        }
        else if (npc.CompareTag("Boss"))
        {
            if (npc.GetComponent<AI_Boss>().isCanKill)
            {
                isGameEnd = true;
            }
        }
        else if (npc.CompareTag("Npc"))
        {
            //Target
            if (npc.GetComponent<AI_Npc>().isCanKill)
            {
                if (player!= null)
                {
                    player.GetComponent<AI_Enemy>().isDie = true;
                    npc.GetComponent<AI_Npc>().enemys.Remove(player);
                }
            }
            else
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    nextState = new Wander(npc, agent, null, statusText);
                    stateEvent = EventState.Exit;
                }
            }
        }
    }

    public override void Exit()
    {
        if (npc.CompareTag("Enemy"))
        {
            npc.GetComponent<AI_Enemy>().isAttack = false;
        }
        else if (npc.CompareTag("Boss"))
        {
            npc.GetComponent<AI_Boss>().isAttack = false;
        }
        else if (npc.CompareTag("Npc"))
        {
            npc.GetComponent<AI_Npc>().isAttack = false;
        }
        
        base.Exit();
    }

}