using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    public string currentStage;
    public string walk = "m_pistol_run";
    public string shoot = "m_pistol_shoot";
    public string idle = "m_pistol_idle_A";
    public string reload = "m_idle_A";
    public string isDie = "m_death_A";
    

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
        if (State.isGameEnd)
        {
            PlayAnim(isDie);
        }
        else if (PlayerController.isShooting)
        {
            PlayAnim(shoot);
        }
        else if (PlayerController.isWalking)
        {
            PlayAnim(walk);
        }
        else if (!PlayerController.isShooting && !PlayerController.isWalking && !PlayerController.isReload)
        {
            PlayAnim(idle);
        }
        else if (PlayerController.isReload)
        {
            PlayAnim(reload);
        }
    }

    private void FixedUpdate()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(shoot) && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            PlayerController.isShooting = false;
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(isDie) && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Animation();
    }
}

