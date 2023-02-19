using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class NpcAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    private AI_Npc aiNpc;
    
    public string currentStage;
    public string idle = "f_melee_combat_idle";
    public string attack = "f_melee_combat_attack_A";
    public string run = "f_melee_combat_run";
    public string die = "f_death_A";
    public string walk = "f_walk";

    private void Start()
    {
        aiNpc = GetComponent<AI_Npc>();
    }

    private void PlayAnim(string newStage)
    {
        if (currentStage == newStage)
        {
            return;
        }
        playerAnimator.Play(newStage);
        currentStage = newStage;
    }

    private void Animation()
    {
        if (!GameManager.isGameStart)
        {
            PlayAnim(idle);
        }
        else if (State.isGameEnd)
        {
            PlayAnim(idle);
        }
        else if (aiNpc.isAttack)
        {
            PlayAnim(attack);
        }
        else if (aiNpc.isWalking)
        {
            PlayAnim(walk);
        }
        else if (aiNpc.isRunning)
        {
            PlayAnim(run);
        }
        else if (aiNpc.isIdle)
        {
            PlayAnim(idle);
        }
        else if (!PlayerController.isShooting && !PlayerController.isWalking && !PlayerController.isReload)
        {
            PlayAnim(die);
        }
    }

    private void FixedUpdate()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(attack) &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            aiNpc.isCanKill = false;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(attack) &&
                 playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
        {
            if (!aiNpc.isCanKill)
            {
                aiNpc.PlayParticle();
                aiNpc.isCanKill = true;
            }
        }
    }

    private void Update()
    {
        Animation();
    }
}