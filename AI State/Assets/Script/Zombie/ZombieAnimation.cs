using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    private AI_Enemy aiEnemy;
    private AI_Boss aiBoss;
    
    public string currentStage;
    public string die = "Z_death_A";
    public string walk = "Z_walk";
    public string run = "Z_run";
    public string idle = "Z_idle_A";
    public string attack = "Z_attack_A";

    private void Start()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            aiEnemy = GetComponent<AI_Enemy>();
        }
        else if (gameObject.CompareTag("Boss"))
        {
            aiBoss = GetComponent<AI_Boss>();
        }
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
        if (aiBoss != null)
        {
            if (State.isGameEnd)
            {
                PlayAnim(idle);
                return;
            }
            
            if (aiBoss.isRuning && !aiBoss.isWalking && !aiBoss.isIdle && !aiBoss.isDie)
            {
                PlayAnim(run);
            }
            else if (aiBoss.isWalking && !aiBoss.isAttack && !aiBoss.isRuning && !aiBoss.isIdle && !aiBoss.isDie)
            {
                PlayAnim(walk);
            }
            else if (aiBoss.isAttack && !aiBoss.isDie)
            {
                PlayAnim(attack);
            }
            else if (aiBoss.isDie)
            {
                transform.GetComponent<NavMeshAgent>().isStopped = true;
                PlayAnim(die);
            }
            else if (aiBoss.isIdle)
            {
                PlayAnim(idle);
            }
            return;
        }
        
        if (State.isGameEnd)
        {
            PlayAnim(idle);
            return;
        }
        
        if (aiEnemy.isRuning && !aiEnemy.isWalking && !aiEnemy.isIdle && !aiEnemy.isDie)
        {
            PlayAnim(run);
        }
        else if (aiEnemy.isWalking && !aiEnemy.isAttack && !aiEnemy.isRuning && !aiEnemy.isIdle && !aiEnemy.isDie)
        {
            PlayAnim(walk);
        }
        else if (aiEnemy.isAttack && !aiEnemy.isDie)
        {
            PlayAnim(attack);
        }
        else if (aiEnemy.isDie)
        {
            transform.GetComponent<NavMeshAgent>().isStopped = true;
            PlayAnim(die);
        }
        else if (aiEnemy.isIdle)
        {
            PlayAnim(idle);
        }
    }

    private void FixedUpdate()
    {
        if (aiBoss != null)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(attack) &&
                playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                aiBoss.isAttack = false;
                aiBoss.isCanKill = false;
            }
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(attack) &&
                     playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
            {
                if (!aiBoss.isCanKill)
                {
                    aiBoss.isCanKill = true;
                }
            }
            
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(die) &&
                playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
            return;
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(attack) &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            aiEnemy.isAttack = false;
            aiEnemy.isCanKill = false;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(attack) &&
                 playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
        {
            if (!aiEnemy.isCanKill)
            {
                aiEnemy.isCanKill = true;
            }
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(die) &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Animation();
    }
}